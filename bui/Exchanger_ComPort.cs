using DataExchange;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bui
{
    public class Exchanger_ComPort : Exchanger<ComPort.StatusEnum>
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        protected ComPort port;
        public Exchanger_ComPort(string portName, int boudrate)
        {
            ComPort.ComProtocol protocol = ComPort.ComProtocol.Default;
            protocol.BoudRate = boudrate;
            port = new ComPort(portName, protocol);
        }
        public override void OnListen()
        {
            
        }

        public override async Task OnRequestAsync()
        {

            var cmd = GetNexCommand();
            var prot = port.Protocol;
            prot.WaitForAnswer = cmd.WaitForAnswer;
            port.Protocol = prot;
            port.Flush();
            if (cmd.IsBroadcast)
            {
                await port.SendBroadcast(cmd.ToArray());
                RemoveCommand(cmd);
               
                return;
            }
            var status = await port.GetRequestToExchangeAsync(cmd.Address);

            if (status != ComPort.StatusEnum.Ok)
            {

                if (BaseCommands.Count == 4)
                {
                    Console.WriteLine($"{cmd.Address} STX not confirmed ");
                }

                cmd.Data.Set(port.Buffer, status);
                if (cmd.CompletionSource != null)
                    Console.WriteLine($"{cmd.Address} {status}");
                ErrorEvent(cmd);
                return;
            }
           
            status = await port.SendPocketAsync(cmd.ToArray(), cmd.Address);
            if (status != ComPort.StatusEnum.Ok)
            {
                cmd.Data.Set(port.Buffer, status);
                ErrorEvent(cmd);
                return;
            }

            
            status = await port.ReceiveSafelyAsync(cmd.Address);

            if (status != ComPort.StatusEnum.Ok)
            {
                if (BaseCommands.Count == 4)
                {
                    Console.WriteLine($"{cmd.Address} {status}");
                }
                cmd.Data.Set(port.Buffer, status);
                ErrorEvent(cmd);
                return;
            }

            cmd.Data.Set(port.Buffer, status);
            DataReceiveEvent(cmd);
        }

        public override void OnStop() => port.Close();
    }
}
