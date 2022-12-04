using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Small_World
{
    class SmallWorld
    {

        //Attributes
        public static List<Movie> allMovies = new List<Movie>(); //list of class movies 
        public static HashSet<string> uniqeActors = new HashSet<string>(); //list of all actors 
        public static Dictionary<string, int> actors_as_int = new Dictionary<string, int>();
        public static Dictionary<int, string> actors_as_strings = new Dictionary<int, string>();

        public static List<Dictionary<int, int>> Actors_adjlist = new List<Dictionary<int, int>>();
        public static List<Dictionary<string, string>> Actor_MoviePair = new List<Dictionary<string, string>>();


        #region Reading TestCases Files To Work On It.
        public static string[] actorsLines;
        public static string[] solutionLines;
        public readonly string textFile;
        public SmallWorld(int choice)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            if (choice == 1)
            {
                textFile = @"Testcases\Sample\movies1.txt";
                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Sample\queries1.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Sample\queries1 - Solution.txt");

            }
            else if (choice == 2)
            {
                textFile = @"Testcases\Complete\small\Case1\Movies193.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\small\Case1\queries110.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\small\Case1\Solution\queries110 - Solution.txt");
            }
            else if (choice == 3)
            {
                textFile = @"Testcases\Complete\small\Case2\Movies187.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\small\Case2\queries50.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\small\Case2\Solution\queries50 - Solution.txt");
            }
            else if (choice == 4)
            {
                textFile = @"Testcases\Complete\medium\Case1\Movies967.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case1\queries85.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case1\Solutions\queries85 - Solution.txt");

            }
            else if (choice == 44)
            {
                textFile = @"Testcases\Complete\medium\Case1\Movies967.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case1\queries4000.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case1\Solutions\queries4000 - Solution.txt");

            }
            else if (choice == 5)
            {
                textFile = @"Testcases\Complete\medium\Case2\Movies4736.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case2\queries110.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case2\Solutions\queries110 - Solution.txt");

            }
            else if (choice == 55)
            {
                textFile = @"Testcases\Complete\medium\Case2\Movies4736.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case2\queries2000.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case2\Solutions\queries2000 - Solution.txt");
            }
            else if (choice == 6)
            {
                textFile = @"Testcases\Complete\large\Movies14129.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\large\queries26.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\large\Solutions\queries26 - Solution.txt");
            }
            else if (choice == 66)
            {
                textFile = @"Testcases\Complete\large\Movies14129.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\large\queries600.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\large\Solutions\queries600 - Solution.txt");
            }
            else if (choice == 7)
            {
                textFile = @"Testcases\Complete\extreme\Movies122806.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\extreme\queries22.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\extreme\Solutions\queries22 - Solution.txt");
            }
            else if (choice == 77)
            {
                textFile = @"Testcases\Complete\extreme\Movies122806.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\extreme\queries200.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\extreme\Solutions\queries200 - Solution.txt");
            }
            #endregion


            CreateAdjacencyList_MoviesActors();
            CreateAdjacencyList_Actors();

            // variables to store source and destination(Handle queries)
            for (int i = 0; i < actorsLines.Length; i++)
            {
                int firstIndex = actorsLines[i].IndexOf('/');
                string actor1 = actorsLines[i].Substring(0, firstIndex);
                string actor2 = actorsLines[i].Substring(firstIndex + 1);
                Bfs_Visit(actor1, actor2);
            }
            watch.Stop();
            var time = watch.Elapsed;

            Console.WriteLine($"Execution Time: {time} ms");

                           }
        public void Initialize()
        {
            for (int i = 0; i < uniqeActors.Count(); i++)
            {
                Actors_adjlist.Add(new Dictionary<int, int>());
                Actor_MoviePair.Add(new Dictionary<string, string>());
            }
        }


        public void CreateAdjacencyList_MoviesActors()
        {
            using (StreamReader file = new StreamReader(textFile))
            {
                string movie;
                while ((movie = file.ReadLine()) != null)
                {
                    Movie m = new Movie();
                    //Extract the movie name
                    int firstIndex = movie.IndexOf('/');
                    string movieName = movie.Substring(0, firstIndex);
                    m.NameOfMovie = movieName;

                    //remove the movie name from the line
                    movie = movie.Remove(0, firstIndex + 1);

                    //put the actors as values to the key which is the movie
                    string[] actorsList = movie.Split('/');
                    foreach (var actor in actorsList)
                    {
                        uniqeActors.Add(actor);
                        m.actors.Add(actor);
                    }

                    allMovies.Add(m);
                }
                file.Close();
                Console.WriteLine($"File Ended");
            }

            int ID = 0;
            foreach (var item in uniqeActors)
            {
                actors_as_int.Add(item.ToString(), ID); //giving each actor an ID (easier and faster to deal with later)

                actors_as_strings.Add(ID, item.ToString()); // the opposite to the above statement
                ID++;
            }

        }


        public void CreateAdjacencyList_Actors()
        {
            Initialize();


            foreach (Movie movie in allMovies)
            {
                int first = 0;
                int movie_actorsL = movie.actors.Count();
                while (first < movie_actorsL)
                {
                    for (int second = 0; second < movie_actorsL; second++)
                    {
                        int secondActor = actors_as_int[movie.actors[second]];
                        int firstActor = actors_as_int[movie.actors[first]];

                        //checking if the two actors is not the same
                        if (movie.actors[first] != movie.actors[second])
                        {
                            //checking if they have not been discovered with each other before
                            if (Actors_adjlist[firstActor].ContainsKey(secondActor) == false)
                            {
                                Actor_MoviePair[firstActor].Add(actors_as_strings[secondActor], movie.NameOfMovie);

                                //saving that these two actors have performed together
                                Actors_adjlist[firstActor].Add(secondActor, 1);
                            }

                            //checking if they have been discovered with each other before
                            else if (Actors_adjlist[firstActor].ContainsKey(secondActor) == true)
                            {
                                Actors_adjlist[firstActor][secondActor]++; //increment the no. Movies they have performed in together
                            }

                        }
                    }

                    first++;
                }

            }
        }

        public Dictionary<int, int> parent = new Dictionary<int, int>();


        //Apply BFS algorithm to find the shortest path between source and desination vertices
        public void Bfs_Visit(string source, string destination)
        {
            Dictionary<int, int> distances = new Dictionary<int, int>();
            Dictionary<int, int> RS = new Dictionary<int, int>();
            Dictionary<int, string> color = new Dictionary<int, string>();

            Queue<int> q = new Queue<int>();

            int source_int = actors_as_int[source];
            int destination_int = actors_as_int[destination];

            //loop to initialize the distance array with maximum values
            for (int i = 0; i < uniqeActors.Count; i++)
            {
                distances[i] = 2000000000;
                color[i] = "white";
                RS.Add(i, 0);
            }

            //add the source in the queue
            q.Enqueue(source_int);

            //initialization related to the source vertex
            parent[source_int] = -1;
            distances[source_int] = 0;
            color[source_int] = "gray";

            bool E = false;
            while (q.Count() != 0)
            {

                int u = q.Dequeue();
                if (u == destination_int && E == true)
                    break;

                foreach (KeyValuePair<int, int> vertex in Actors_adjlist[u])
                {

                    if (color[vertex.Key] == "white") //still not visited yet, let's visit it
                    {
                        color[vertex.Key] = "gray"; //mark it as visited
                        distances[vertex.Key] = distances[u] + 1;

                        parent[vertex.Key] = u;

                        q.Enqueue(vertex.Key);
                    }


                    if (vertex.Key != destination_int && distances[u] < distances[vertex.Key])
                    {
                        if (RS[u] + Actors_adjlist[u][vertex.Key] > RS[vertex.Key])
                            RS[vertex.Key] = RS[u] + Actors_adjlist[u][vertex.Key];

                    }
                    else if (vertex.Key == destination_int && distances[u] < distances[destination_int])
                    {
                        if (RS[u] + Actors_adjlist[u][vertex.Key] > RS[vertex.Key])
                        {

                            RS[vertex.Key] = RS[u] + Actors_adjlist[u][vertex.Key];
                            parent[vertex.Key] = u;
                            E = true;
                            break;

                        }
                    }

                }
                color[u] = "black"; //mark it as fully discovered

            }

            int actor1_Int;
            int actor2_Int;

            Console.WriteLine("Degree Of Separation: \tRelation Strength: ");
            Console.WriteLine(distances[destination_int] + "\t\t\t\t\t" + RS[destination_int]);
            List<int> path = new List<int>();
            int src = actors_as_int[source];
            Find_Path(destination_int, path, parent);
            path.Add(src);
            path.Reverse();

            Console.Write("Chain of Actors: ");
            for (int i = 0; i < path.Count; i++)
            {
                Console.Write(actors_as_strings[path[i]]);
                if ((i != path.Count - 1))
                    Console.Write(" -> ");
            }

            Console.WriteLine();
            Console.Write("Chain Of Movies: ");

            for (int i = 0; i < path.Count; i++)
            {
                if (i + 1 == path.Count)
                {
                    break;
                }
                actor1_Int = path[i];
                actor2_Int = path[i + 1];

                Console.Write(Actor_MoviePair[actor1_Int][actors_as_strings[actor2_Int]]);
                if ((i + 2 != path.Count))
                    Console.Write(" => ");
            }
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------------------------------");

        }

        public void Find_Path(int destination, List<int> path, Dictionary<int, int> parent)
        {
            try
            {
                //base_case
                if (parent[destination] == -1)
                    return;

                int p = parent[destination];
                path.Add(destination);
                Find_Path(p, path, parent);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }

}