using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exercise
{
    public class GeneA : GeneAbstract<int>
    {

        /**
         * @return calculates and returns the fitness value of the individual
         */
        public override float CalculateFitness()
        {
            string pheno = GetPhenotype();
            int fitness = 0;
            foreach (var i in pheno) {
                if (i == 'A')
                    fitness++;
            }
            return fitness;
        }

        /**
         * Corresponds the chromosome encoding to the phenotype, which is a representation
         * that can be read, tested and evaluated by the main program.
         * @return a String with a length equal to the chromosome size, composed of "A"s
         * at the positions where the chromosome is 1 and "a"s at the posiitons
         * where the chromosome is 0
         */
        public override string GetPhenotype()
        {
            string output = "";
            foreach (var i in mChromosome) {
                if (i == 1) {
                    output += "A";
                }
                else {
                    output += "a";
                }
            }
            return output;
        }

        /**
         * Mutates a gene using inversion, random mutation or other methods.
         * This function is called after the mutation chance is rolled.
         * Mutation can occur (depending on the designer's wishes) to a parent
         * before reproduction takes place, an offspring at the time it is created,
         * or (more often) on a gene which will not produce any offspring afterwards.
         */
        public override void Mutate()
        {
            // Flip single genome
            int genomeNumber = Random.Range(0, mChromosome.Length);

            if (mChromosome[genomeNumber] == 1)
                mChromosome[genomeNumber] = 0;
            else
                mChromosome[genomeNumber] = 1;
        }

        /**
         * Randomizes the numbers on the mChromosome array to values 0 or 1
         */
        public override void RandomizeChromosome()
        {
            for (int i = 0; i < mChromosome.Length; i++) {
                mChromosome[i] = Random.Range(0, 2);
            }
        }

        /**
         * Creates a number of offspring by combining (using crossover) the current
         * Gene's chromosome with another Gene's chromosome.
         * Usually two parents will produce an equal amount of offpsring, although
         * in other reproduction strategies the number of offspring produced depends
         * on the fitness of the parents.
         * @param other: the other parent we want to create offpsring from
         * @return Array of Gene offspring (default length of array is 2).
         * These offspring will need to be added to the next generation.
         */
        public override GeneAbstract<int>[] Reproduce(GeneAbstract<int> other)
        {
            GeneA[] offspring = new GeneA[2];

            offspring[0].mChromosome = singlePointCrossover(other.mChromosome);
            offspring[1].mChromosome = singlePointCrossover(other.mChromosome);

            return offspring;
        }

        int[] singlePointCrossover(int[] other) {
            int[] output = new int[mChromosome.Length];
            int point = Random.Range(1, mChromosome.Length);
            for (int i = 0; i < output.Length; i++) {
                if (i < point) {
                    output[i] = mChromosome[i];
                }
                else {
                    output[i] = other[i];
                }
            }
            return output;
        }
    }
}
