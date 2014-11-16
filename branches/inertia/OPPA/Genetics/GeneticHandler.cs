using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace OPPA.Genetics
{
    public class GeneticHandler
    {
        private double mediaFIT = 0;
        private Chromosome[] population, children;
        private int populationSize;
        private int generations, generation;
        private double mutationRate;
        private static Random random = new Random(DateTime.Now.Millisecond);

        public GeneticHandler(int populationSize, int generations, double mutationRate, 
            int steps, PointF start, List<PointF> checkpoints, Bitmap world)
        {
            this.populationSize = populationSize;
            this.generations = generations;
            this.mutationRate = mutationRate;
            this.generation = 0;
            InitializePopulation(steps, start, checkpoints, world);
        }

        public void InitializePopulation(int steps, PointF start, List<PointF> checkpoints, Bitmap world)
        {
            bool[,] map = MapWorld(world);
            population = new Chromosome[populationSize];
            Parallel.For(0, populationSize, i =>
            {
                population[i] = Chromosome.RandomChromosome(steps, start, checkpoints, map);
            });
        }

        public Chromosome FindSolution()
        {
            Chromosome melhor;

            do
            {
                GenerateChildren();
                Selection(populationSize / 3);
                MutatePopulation();
                generation++;
                melhor = GetBestIndividual();
            } while (melhor.Fitness > 0 &&
                     generation < generations
                    );
            SystemSounds.Beep.Play();
            return melhor;
        }


        public void Selection(int killnumber)
        {
            for (int i = 0; i < populationSize / 2; i++)
            {
                int x = 0;
                do
                {
                    x = random.Next(populationSize);
                    double fator = population[x].Fitness / (mediaFIT * 2);
                    double r = random.NextDouble();
                    if (r < fator) break;
                } while (true);

                population[x] = children[i];
            }
        }

        public void MutatePopulation()
        {
            for (int i = 0; i < populationSize; i++)
            {
                population[i].Mutate(mutationRate);
            }
        }

        public Chromosome GetBestIndividual()
        {
            int min = 0;
            for (int i = 0; i < populationSize; i++)
            {
                if (population[i].Fitness < population[min].Fitness)
                    min = i;
            }
            return population[min];
        }

        public void GenerateChildren()
        {
            children = new Chromosome[populationSize / 2];

            double soma = 0;
            for (int i = 0; i < populationSize / 2; i++)
            {
                children[i] = population[i].Crossover(population[populationSize - i - 1]);
                soma +=
                    population[i].Fitness +
                    population[populationSize - i - 1].Fitness +
                    children[i].Fitness;
            }
            mediaFIT = soma / (populationSize * 1.5);

        }

        private bool[,] MapWorld(Bitmap world)
        {
            bool[,] map = new bool[world.Width, world.Height];
            Color c = Color.FromArgb(127, 127, 127);
            for (int j = 0; j < world.Height; j++)
            {
                for (int i = 0; i < world.Width; i++)
                {
                    map[i, j] = !c.Equals(world.GetPixel(i, j));
                }
            }
            return map;
        }
    }
}
