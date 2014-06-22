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
        public int priority { get; set; }
        public Process() {
            
            Random rd = new Random();
            this.Id = rd.Next(0, 100);
            this.duration = rd.Next(1000, 5000);
            Console.WriteLine("Has generate the process " + this.Id);
        }
    }
    #endregion

    #region ProcessCentral
    public class ProcessCentral {
        public static Queue<Process> FIFO = new Queue<Process>();
        public static Queue<Process> RR = new Queue<Process>();
        public void StartProcessing() {
            ShowFIFO();
            ShowRR();
         
            Processor _pc = new Processor();
            Thread Processor = new Thread(_pc.DoProcessorWork);
            Processor.IsBackground = true;
            Processor.Start();
        }
        public static void ShowFIFO() {
            Console.WriteLine("Showing FIFO List");
            foreach(Process pc in FIFO)  {
                Console.WriteLine("Process -> " + pc.Id + " Duration ->" + pc.duration);
            }
            

        }
        public static void ShowRR()
        {
            Console.WriteLine("Showing Round Robin List");
            foreach (Process pc in RR)
            {
                Console.WriteLine("Process -> " + pc.Id + " Duration ->" + pc.duration);
            }


        }
        public void GenerateProcessWork()
        {
            int count = 0;
            while (true)
            {
                count++;
                if (count < 6)
                {
                    Process newProcessFIFO = new Process();
                    FIFO.Enqueue(newProcessFIFO);
                    newProcessFIFO.priority = 1;
                    Console.WriteLine("The process " + newProcessFIFO.Id + " has been added to FIFO Queue because his priority is " + newProcessFIFO.priority);
                    Thread.Sleep(1000);
                    Process newProcessRR = new Process();
                    newProcessRR.priority = 2;
                    RR.Enqueue(newProcessRR);
                    Console.WriteLine("The process " + newProcessRR.Id + " has been added to Round Robin Queue because his priority is " + newProcessRR.priority);
                    if (FIFO.Count % 5 == 0)
                    {
                        StartProcessing();
                        Thread.Sleep(2000000);
                    }

                    Thread.Sleep(2000);
                }
            }
        }
    }


    #endregion
    public class Processor {
        public bool isBusy { get; set; }
        public int durationBusy { get; set; }
        public Process inProcessorNow { get; set;}
        public void DoProcessorWork() {
            bool fifo = true;
            int totalTimeRR = 0;
             int totalDurationFIFO = 0;
            while (true)
            {
                if (fifo)
                {
                    if (ProcessCentral.FIFO.Count > 0)
                    {
                       
                        if (inProcessorNow != null)
                        {
                            Console.WriteLine("The process " + inProcessorNow.Id + " has been out of process with his duration of " + inProcessorNow.duration);
                            ProcessCentral.ShowFIFO();
                        }
                        Process process = ProcessCentral.FIFO.Dequeue();
                        inProcessorNow = process;
                        Console.WriteLine("The process " + process.Id + " was dequeue with the duration " + process.duration);
                        isBusy = true;
                        durationBusy = process.duration;
                        totalDurationFIFO += durationBusy;
                        Thread.Sleep(durationBusy);
                    }
                    else {
                        fifo = false;

                        
                    }
                }
                else {
                    if (ProcessCentral.RR.Count > 0)
                    {
             
                        if (inProcessorNow != null)
                        {
                            Console.WriteLine("The process " + inProcessorNow.Id + " has been out of process with his duration of " + inProcessorNow.duration);
                            ProcessCentral.ShowRR();
                        }
                        Process process = ProcessCentral.RR.Dequeue();
                        inProcessorNow = process;
                        Console.WriteLine("The process " + process.Id + " was dequeue with the duration " + process.duration);
                        isBusy = true;
                        if (process.duration > 2000)
                        {
                            process.duration = process.duration - 2000;
                            ProcessCentral.RR.Enqueue(process);
                            Console.WriteLine("The process " + process.Id + " has been enqueue again because his duration is biggest than the round");
                            totalTimeRR += 2000;
                            Thread.Sleep(2000);
                          
                        }
                        else
                        {
                            totalTimeRR += process.duration;
                            Thread.Sleep(process.duration);
                        }
                    }
                    else {
                        Console.WriteLine("The FIFO and Round Robin Queue are empty.");
                        Console.WriteLine("The FIFO total processing time was " + totalDurationFIFO.ToString() + " miliseconds");
                        Console.WriteLine("The Round Robin total processing time was " + totalTimeRR + " miliseconds");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }

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
            
            Console.ReadLine();
        }

       
    }
}
