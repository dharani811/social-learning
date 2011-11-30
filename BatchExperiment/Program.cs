﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using VisualizeWorld;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using System.IO;
using System.Threading;


namespace BatchExperiment
{
    class Program
    {

         SimpleExperiment _experiment;
         NeatEvolutionAlgorithm<NeatGenome> _ea;
         int MaxGenerations = 20;
         string _filename;
         bool finished = false;


        static void Main(string[] args)
        {
            int numRuns = args.Length > 0 ? int.Parse(args[0]) : 5;
            Program[] r = new Program[numRuns * 2];
            for (int i = 0; i < numRuns; i++)
            {
                Program p = new Program();
                p.RunExperiment(@"..\..\..\experiments\neural.config.xml", @"..\..\..\experiments\neural_results" + i + ".txt");
                Program q = new Program();
                q.RunExperiment(@"..\..\..\experiments\social.config.xml", @"..\..\..\experiments\social_results" + i + ".txt");
                r[2 * i] = p;
                r[2 * i + 1] = q;
            }
            while (!AllFinished(r)) Thread.Sleep(1000);
        }

         static bool AllFinished(Program[] programs)
         {
             foreach (Program p in programs)
                 if (!p.finished)
                     return false;
             return true;
         }

         void RunExperiment(string XMLFile, string filename)
         {
             _filename = filename;
             _experiment = new SimpleExperiment();
             // Load config XML.

             using (TextWriter writer = new StreamWriter(_filename))
             {

                 writer.WriteLine("generation,averageFitness,topFitness");
             }

             XmlDocument xmlConfig = new XmlDocument();
             xmlConfig.Load(XMLFile);
             _experiment.Initialize("SimpleEvolution", xmlConfig.DocumentElement);
             _experiment.World.PlantLayoutStrategy = social_learning.PlantLayoutStrategies.Spiral;

             startEvolution();

         }

        void startEvolution()
        {

            // Create evolution algorithm and attach update event.
            _ea = _experiment.CreateEvolutionAlgorithm();
            _ea.UpdateEvent += new EventHandler(_ea_UpdateEvent);
            // Start algorithm (it will run on a background thread).
            _ea.StartContinue();
        }

        void _ea_UpdateEvent(object sender, EventArgs e)
        {
            using (TextWriter writer = new StreamWriter(_filename, true))
            {
                var averageFitness = _ea.GenomeList.Average(x => x.EvaluationInfo.Fitness);
                var topFitness = _ea.CurrentChampGenome.EvaluationInfo.Fitness;
                var generation = _ea.CurrentGeneration;
                Console.WriteLine(generation + "," + averageFitness + "," + topFitness);
                writer.WriteLine( generation + "," +  averageFitness + "," + topFitness);
                if (_ea.CurrentGeneration >= MaxGenerations)
                {
                    _ea.Stop();
                    finished = true;
                }
            }
        }
        
    }
}
