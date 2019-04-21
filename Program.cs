using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TestConcole
{
    class Program
    {
        static List<string> catalogCycles;
        static List<Vertex> V;
        static List<Edge> E;

        static void Main(string[] args)
        {
            catalogCycles = new List<string>();
            V = new List<Vertex>();
            E = new List<Edge>();
            int countCord = 0;
            Reader2(countCord, E);
            //V.Add(new Vertex(0, 0));
            //V.Add(new Vertex(1, 1));
            //V.Add(new Vertex(2, 2));
            //V.Add(new Vertex(3, 3));
            //E.Add(new Edge(0, 1));
            //E.Add(new Edge(0, 2));
            //E.Add(new Edge(1, 0));
            //E.Add(new Edge(1, 2));
            //E.Add(new Edge(2, 0));
            //E.Add(new Edge(2, 1));
            //E.Add(new Edge(2, 3));
            //E.Add(new Edge(3, 2));
            СyclesSearch();
            WriteRezlt(catalogCycles);
        }

       

        static void BildEdge(List<Edge> E, int[] nums)
        {
            int j = 0;
            for (int i = 1; i < nums.Length; i++)
            {
                if (nums[i] == 0)
                {
                    j++;
                }
                else
                {
                    int temp = nums[i] - 1;
                    E.Add(new Edge(j, temp));
                }
            }
        }

        static void BildVert(List<Vertex> V, int countCord)
        {
            for (int i = 0; i < countCord; i++)
            {
                V.Add(new Vertex(i, i));
            }
        }

        private static void СyclesSearch()
        {
            int[] color = new int[V.Count];
            for (int i = 0; i < V.Count; i++)
            {
                for (int k = 0; k < V.Count; k++)
                    color[k] = 1;
                List<int> cycle = new List<int>();
                cycle.Add(i + 1);
                DFScycle(i, i, E, color, -1, cycle);
            }
        }

        private static void DFScycle(int u, int endV, List<Edge> E, int[] color, int unavailableEdge, List<int> cycle)
        {
            if (u != endV)       //если u == endV, то эту вершину перекрашивать не нужно, иначе мы в нее не вернемся, а вернуться необходимо
                color[u] = 2;
            else if (cycle.Count >= 3)
            {
                cycle.Reverse();
                string s = cycle[0].ToString();
                for (int i = 1; i < cycle.Count; i++)
                {
                    s += "-" + cycle[i].ToString();
                }
                bool flag = false;      //есть ли палиндром для этого цикла графа в List<string> catalogCycles?

                for (int i = 0; i < catalogCycles.Count; i++)

                    if (catalogCycles[i].ToString() == s)
                    {
                        flag = true;
                        break;
                    }
                if (!flag)
                {
                    if (cycle.Count > 3)
                    {
                        cycle.Reverse();
                        s = cycle[0].ToString();
                        for (int i = 1; i < cycle.Count; i++)
                            s += "-" + cycle[i].ToString();
                        catalogCycles.Add(s);
                    }
                }
                return;
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (w == unavailableEdge)
                    continue;
                if (color[E[w].v2] == 1 && E[w].v1 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(E[w].v2 + 1);
                    DFScycle(E[w].v2, endV, E, color, w, cycleNEW);
                    color[E[w].v2] = 1;
                }
                else if (color[E[w].v1] == 1 && E[w].v2 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(E[w].v1 + 1);
                    DFScycle(E[w].v1, endV, E, color, w, cycleNEW);
                    color[E[w].v1] = 1;
                }
            }
        }
        private static void WriteRezlt(List<string> catalogCycles)
        {
            using (var sw = File.CreateText(Path.GetFileName("out.txt")))
            {
                if (catalogCycles.Count > 0)
                {
                    sw.WriteLine('N');
                    sw.Write(catalogCycles.ElementAt(0));
                }
                else
                    sw.WriteLine('A');
            }
            return;
        }

        static void Reader2(int countCord, List<Edge> E)
        {
            string file = File.ReadAllText(Path.GetFileName("in.txt") );
            int[] nums = file
            .Split(new char[] { ' ', '\n','\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(n => int.Parse(n))
            .ToArray();
            countCord = nums[0];
            BildVert(V, countCord);
            BildEdge(E, nums);
        }
    }

    class Vertex
    {
        public int x, y;

        public Vertex(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Edge
    {
        public int v1, v2;

        public Edge(int v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
    }
}