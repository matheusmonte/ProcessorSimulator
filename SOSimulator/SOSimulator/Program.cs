using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SOSimulator
{
    #region Process
    public class Process {
        public int duration { get; set; }
        public int Id { get; set; }
        public Process() {
            
            Random rd = new Random();
            this.Id = rd.Next(0, 100);
            this.duration = rd.Next(1000, 5000);
            Console.WriteLine("Foi gerado o processo " + this.Id);
        }
    }
    #endregion

    #region ProcessCentral
    public class ProcessCentral {
        public static Queue<Process> FIFO = new Queue<Process>();

        public void ShowFIFO() {
            Console.WriteLine("Showing FIFO List");
            foreach(Process pc in FIFO)  {
                Console.WriteLine("Process -> " + pc.Id + " Duration ->" + pc.duration);
            }
        }
        public void GenerateProcessWork()
        {
            while (true)
            {
                Process newProcess = new Process();
                FIFO.Enqueue(newProcess);
                Console.WriteLine("O processo " + newProcess.Id + " foi adicionado ao FIFO");
                if (FIFO.Count % 5 == 0)
                    ShowFIFO();
                Thread.Sleep(2000);
            }
        }
    }


    #endregion
    public class Processor {
        public bool isBusy { get; set; }
        public int durationBusy { get; set; }

        public void DoProcessorWork() {
            while (true)
            {
                if (ProcessCentral.FIFO.Count > 0)
                {
                    Process process = ProcessCentral.FIFO.Dequeue();
                    Console.WriteLine("The process " + process.Id + " was dequeue with the duration " + process.duration);
                    isBusy = true;
                    durationBusy = process.duration;
                    Thread.Sleep(durationBusy);
                }
                else {
                    Console.WriteLine("The Queueu is empty");
                    Thread.Sleep(2000);
                }
            }
        }

    }
    public class Program
    {
        
        
        static void Main(string[] args)
        {
            ProcessCentral pg = new ProcessCentral();
            Thread GenerateProcess = new Thread(pg.GenerateProcessWork);
           // GenerateProcess.IsBackground = true;
            GenerateProcess.Start();
            Processor pc = new Processor();
            Thread Processor = new Thread(pc.DoProcessorWork);
            Processor.IsBackground = true;
            Processor.Start();
                      
            
        }

       
    }
}
