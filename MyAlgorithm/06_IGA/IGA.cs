using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonClass;

namespace _06_IGA
{
    internal class IGA
    {
        /// <summary>
        /// 迭代次数
        /// </summary>
        public int Iteration { get; set; } = 200;
        /// <summary>
        /// 种群数量
        /// </summary>
        public int NP { get; set; } = 50;
        /// <summary>
        /// 交叉算子
        /// </summary>
        public double PC { get; set; } = 0.9;
        /// <summary>
        /// 变异算子
        /// </summary>
        public double PM { get; set; } = 0.3;
        /// <summary>
        /// 基因
        /// </summary>
        private List<Gene> Genes { get; set; }


        public void Main()
        {
            //1.初始化种群
            var population=InitPopulation();
            //2.计算适应度


            int count = 1;
            var parents=population.Individuals.DeepCloneInds();
            while (count < Iteration)
            {
                //选择
                var selects = Select(parents);
                //交叉
                var childs=Crossover(selects);
                //变异
                Mutation(childs);
                //合并种群
                var mergeInds=MergePopulation(parents,childs);
                //精英保留
                var elitisms=Elitism(mergeInds);
                //迭代
                parents=elitisms.DeepCloneInds();
                count++;
            }
            //结果查看
        }

        /// <summary>
        /// 初始化种群
        /// </summary>
        /// <returns></returns>
        private Population InitPopulation()
        {
            Individual[] individuals = new Individual[NP];

            //第一层循环：种群
            for (int i = 0; i < NP; i++)
            {
                Gene[] genes = new Gene[Genes.Count];
                //第二层循环：基因
                for (int j = 0; j < Genes.Count; j++)
                {
                    var gene = Genes[j];
                    //生成随机种子
                    byte[] buffer = Guid.NewGuid().ToByteArray();
                    int seed = BitConverter.ToInt32(buffer, 0);
                    Random random = new Random(seed);
                    int randomIndex = random.Next(gene.Range.Length);
                    gene.Value = gene.Range[randomIndex];
                    genes[j] = gene;
                }
                //一条染色体
                Gene[] clones = new Gene[Genes.Count];
                for (int t = 0; t < genes.Length; t++)
                {
                    var clone = genes[t].DeepCloneGene();
                    clones[t] = clone;
                }
                Individual individual = new Individual(clones);
                individuals[i] = individual;
            }
            //种群
            Population population = new Population(individuals);
            return population;
        }



        /// <summary>
        /// 选择
        /// 策略：锦标赛
        /// </summary>
        /// <param name="inds"></param>
        /// <returns></returns>
        private List<Individual> Select(List<Individual> inds)
        {
            List<Individual> saves=new List<Individual>();
            for (int i = 0; i < NP; i++)
            {
                //每次随机选择10个个体比较
                byte[] buffer = Guid.NewGuid().ToByteArray();
                int seed = BitConverter.ToInt32(buffer, 0);
                Random random = new Random(seed);
                int[] indexs=new int[10];
                List<Individual> selects = new List<Individual>();
                for (int j = 0; j < 10; j++)
                {
                    indexs[j]=random.Next(0, NP);
                    var select = inds[indexs[j]];
                    selects.Add(select);
                }
                //从这10个染色体中，选择最好的保留下来（pareto等级最低，拥挤度最高）
                var best = selects.OrderBy(p => p.Function).FirstOrDefault();
                saves.Add(best);
            }
            return saves;

        }

        /// <summary>
        /// 交叉
        /// 策略：两点交叉
        /// </summary>
        /// <param name="inds"></param>
        /// <returns></returns>
        private List<Individual> Crossover(List<Individual> inds)
        {
            List<Individual> childs = new List<Individual>();
            for (int i = 0; i < NP; i++)
            {
                //生成随机种子
                byte[] buffer = Guid.NewGuid().ToByteArray();
                int seed = BitConverter.ToInt32(buffer, 0);
                Random random = new Random(seed);
                var rand = random.NextDouble();
                //小于交叉概率进行交叉
                if (rand < PC)
                {
                    //随机选出两个个体
                    byte[] buffer2 = Guid.NewGuid().ToByteArray();
                    int seed2 = BitConverter.ToInt32(buffer2, 0);
                    Random rnd = new Random(seed2);
                    int index1= rnd.Next(0, NP);
                    int index2 = rnd.Next(0, NP);
                    //选出两个不同的个体
                    while (index1 == index2)
                    {
                        index2= rnd.Next(0, NP);
                    }
                    //父代
                    var parent1 = inds[index1];
                    var parent2 = inds[index2];
                    //多点交叉（摒弃SBX、NDX交叉算子）
                    var crossPt1 = rnd.Next(0, parent1.Genes.Length);
                    var crossPt2 = rnd.Next(0, parent1.Genes.Length);
                    while (crossPt1 == crossPt2)
                    {
                        crossPt2=rnd.Next(0, parent1.Genes.Length);
                    }
                    //2点交叉可产生6个子代，这里只交换中间基因，使之产生2个子代
                    if (crossPt2 < crossPt1)
                    {
                        int temp= crossPt1;
                        crossPt1 = crossPt2;
                        crossPt2 = temp;
                    }

                    var seg1 = parent1.Genes.SegArraryToThreeParts(crossPt1, crossPt2);
                    var seg2 = parent2.Genes.SegArraryToThreeParts(crossPt1, crossPt2);
                    var gen1 = seg1[0].Concat(seg2[1]).Concat(seg1[2]).ToArray();
                    var gen2 = seg2[0].Concat(seg1[1]).Concat(seg2[2]).ToArray();
                    Individual child1 = new Individual(gen1);
                    Individual child2 = new Individual(gen2);
                    childs.Add(child1);
                    childs.Add(child2);

                }

            }
            return childs;
        }

        /// <summary>
        /// 变异
        /// 策略：两点变异
        /// </summary>
        /// <param name="inds"></param>
        private void Mutation(List<Individual> inds)
        {
            for (int i = 0; i < NP; i++)
            {
                //生成随机种子
                byte[] buffer = Guid.NewGuid().ToByteArray();
                int seed = BitConverter.ToInt32(buffer, 0);
                Random random = new Random(seed);
                var rand = random.NextDouble();
                //小于编译概率进行编译
                if (rand < PM)
                {
                    //随机选出1个个体
                    byte[] buffer2 = Guid.NewGuid().ToByteArray();
                    int seed2 = BitConverter.ToInt32(buffer2, 0);
                    Random rnd = new Random(seed2);
                    int index = rnd.Next(0, NP);
                    var parent = inds[index];
                    //两点变异（摒弃高斯变异算子、混合变异算子）
                    var muPt1 = rnd.Next(0, parent.Genes.Length);
                    var muPt2 = rnd.Next(0, parent.Genes.Length);
                    //选出两个不同的个体
                    while (muPt1 == muPt2)
                    {
                        muPt2 = rnd.Next(0, parent.Genes.Length);
                    }
                    var cloneGenes = parent.Genes.ToList().Select(p => p.DeepCloneGene()).ToList();
                    //随机变异
                    Random rand2 = new Random();
                    int randIdx1 = rand2.Next(cloneGenes[muPt1].Range.Length);
                    int randIdx2 = rand2.Next(cloneGenes[muPt2].Range.Length);
                    cloneGenes[muPt1].Value = cloneGenes[muPt1].Range[randIdx1];
                    cloneGenes[muPt2].Value = cloneGenes[muPt2].Range[randIdx2];
                    //变异后的染色体加入孩子列表，同时孩子列表删除编译前的父亲染色体
                    Individual mutation=new Individual(cloneGenes.ToArray());
                    inds.Add(mutation);
                    inds.Remove(parent);

                }
            }
        }

        /// <summary>
        /// 父代、子代合并
        /// </summary>
        /// <param name="parents"></param>
        /// <param name="childs"></param>
        /// <returns></returns>
        private List<Individual> MergePopulation(List<Individual> parents,List<Individual> childs)
        {
            parents.AddRange(childs);
            return parents;
        }

        /// <summary>
        /// 精英保留
        /// </summary>
        /// <param name="inds"></param>
        /// <returns></returns>
        private List<Individual> Elitism(List<Individual> inds)
        {
            //排序：pareto等级越低，拥挤度越大，则该染色体越优秀
            var sorts = inds.OrderBy(p => p.Function).ToList();
            var elitisms = sorts.Take(NP).ToList();
            return elitisms;
        }

    }
}
