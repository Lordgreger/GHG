using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extra
{
    public class GenePixel : GeneAbstract<Color>
    {
        public override float CalculateFitness(int resolution, SpriteRenderer renderer)
        {
            mFitness = ImageWriter.CompareImage(resolution, mChromosome, renderer);
            return mFitness;
        }

        public override void Mutate()
        {
            for (int i = 0; i < mChromosome.Length; i++)
            {
                if (Random.Range(0f, mChromosome.Length) < 1f)
                {
                    if (Random.Range(0,2) == 0)
                        mChromosome[i] += Random.ColorHSV() * 0.1f;
                    else
                        mChromosome[i] -= Random.ColorHSV() * 0.1f;
                }
            }
        }

        public override void RandomizeChromosome()
        {
            // code for randomization of initial weights goes HERE
            for (int i = 0; i < mChromosome.Length; i++)
            {
                mChromosome[i] = Random.ColorHSV();
            }
        }

        public override GenePixel[] Reproduce<GenePixel>(GenePixel other)
        {
            GenePixel[] result = new GenePixel[2];
            result[0] = new GenePixel();
            result[1] = new GenePixel();
            // initilization of offspring chromosome goes HERE
            int cut = Random.Range(0, mChromosome.Length);
            for (int i = 0; i < mChromosome.Length; i++)
            {
                if (i < cut)
                {
                    result[0].mChromosome[i] = mChromosome[i];
                    result[1].mChromosome[i] = other.mChromosome[i];
                }
                else
                {
                    result[0].mChromosome[i] = other.mChromosome[i];
                    result[1].mChromosome[i] = mChromosome[i];
                }
            }
            return result;
        }

        public override Color[] GetPhenotype()
        {
            return mChromosome;
        }
    }
}
