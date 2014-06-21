using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SOSimulator
{
    public class Process {
        public int duration { get; set; }
        public int Id { get; set; }
        public Process(int duration) {
            this.duration = duration;
            Random rd = new Random();
            this.Id = rd.Next(0, 100);
            Console.WriteLine("Foi gerado o processo " + this.Id);
        }
    }

    public class ProcessCentral {
        List<Process> FIFO { get; set; }

        public ProcessCentral()
        {
            FIFO = new List<Process>();
        }
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
                Process newProcess = new Process(1000);
                FIFO.Add(newProcess);
                Console.WriteLine("O processo " + newProcess.Id + " foi adicionado ao FIFO");
                if (FIFO.Count % 5 == 0)
                    ShowFIFO();
                Thread.Sleep(2000);
            }
        }
    }



    public class Processor {
        public bool isBusy { get; set; }
        public int durationBusy { get; set; }

        public void DoProcessorWork(Process process) {
            while (true)
            {
                isBusy = true;
                durationBusy = process.duration;
                Thread.Sleep(durationBusy);
            }
        }

    }
    public class Program
    {
        
        
        static void Main(string[] args)
        {
            ProcessCentral pg = new ProcessCentral();
            Thread GenerateProcess = new Thread(pg.GenerateProcessWork);
            //GenerateProcess.IsBackground = false;
            GenerateProcess.Start();
           
            
        }

       
    }
}
