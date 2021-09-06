using System;
using System.Collections.Generic;
using System.Text;

namespace SpreadsheetUtilities
{
    // Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
    // Version 1.1 (Fixed error in comment for RemoveDependency.)
    // Version 1.2 - Daniel Kopta 
    //               (Clarified meaning of dependent and dependee.)
    //               (Clarified names in solution/project structure.)
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}  
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>

    public class DependencyGraph
    {
        private int size;
        private Dictionary<string, List<string>[]> graph;    // the List<string>[] hold two values List<string> dependents, List<string> dependees   
        private static readonly int DEPENDENTS_INDEX = 0; // Dependents Index
        private static readonly int DEPENDEES_INDEX = 1;   // Dependees Index


        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            graph = new Dictionary<string, List<string>[]>();
            size = 0;
        }

        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                return size;
            }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// <return>the size of dependees, if the passed in element is null or does not exist in the graph, then it will return -1</return>
        /// </summary>
        public int this[string s]
        {

            get
            {
                if (s == null)
                {
                    return 0;
                }

                if (graph.ContainsKey(s))
                {
                    return graph[s][DEPENDEES_INDEX].Count;
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// If the argument is null or does not exist in the graph, return false automatically
        /// If the argument is not null and exist in the graph, return false if dependents list is empty, return true if is not empty. 
        /// </summary>
        public bool HasDependents(string s)
        {
            if (s == null)
            {
                return false;
            }
            if (graph.ContainsKey(s))
            {
                return graph[s][DEPENDENTS_INDEX].Count > 0; // return true if the dependency list has more than one element. 
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.    
        /// If the argument is null or does not exist in the graph, return false automatically
        /// If the argument is not null and exist in the graph, return false if dependees list is empty, return true if dependees list is not empty. 
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
            {
                return false;
            }

            if (graph.ContainsKey(s))
            {
                return graph[s][DEPENDEES_INDEX].Count != 0; // return true if the dependee list has more than one element. 
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// If the argument is not null and exist in the graph, return dependent list 
        /// If the argument is null or does not exist in the graph, return empty list automatically
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
            {
                return null;
            }

            if (graph.ContainsKey(s))
            {
                return graph[s][DEPENDENTS_INDEX];
            }
            else
            {
                return new List<string>();
            }
        }



        /// <summary>
        /// Enumerates dependees(s).
        /// If the argument is not null and exist in the graph, return dependee list
        /// If the argument is null or does not exist in the graph, return empty list automatically
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
            {
                return null;
            }

            if (graph.ContainsKey(s))
            {
                return graph[s][DEPENDEES_INDEX];
            }
            else
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// <para>Do nothing if value is null </para>
        /// <para>Do nothing if relationship already exist</para>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>   
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                return;
            }

            if (!graph.ContainsKey(s)) // the key does not exist, need to add the key in. 
            {
                List<string> dependent = new List<string>();
                List<string> dependee = new List<string>();
                graph.Add(s, new List<string>[2] { dependent, dependee });
            }

            if (!graph.ContainsKey(t)) // the key does not exist, need to add the key in. 
            {
                List<string> dependent = new List<string>();
                List<string> dependee = new List<string>();
                graph.Add(t, new List<string>[2] { dependent, dependee });
            }

            if (!graph[s][DEPENDENTS_INDEX].Contains(t) && !graph[t][DEPENDEES_INDEX].Contains(s))
            // Only add if they do not have any relationships
            {
                graph[s][DEPENDENTS_INDEX].Add(t);  // s add t to its dependent 
                graph[t][DEPENDEES_INDEX].Add(s);  // t add s to its dependent 
                size++;                 // increment size by 1 because there is an extra pair added
            }
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// Do nothing if dependency does not exist or arguments are null
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if (s == null || t == null)      // s or t are null
            {
                return;
            }

            if (graph.ContainsKey(s) && graph.ContainsKey(t))        // contain s and t
            {
                if (graph[s][DEPENDENTS_INDEX].Contains(t) && graph[t][DEPENDEES_INDEX].Contains(s))
                // contains t and s in dependent/dependee list
                {
                    _ = graph[s][DEPENDENTS_INDEX].Remove(t);       // remove t
                    _ = graph[t][DEPENDEES_INDEX].Remove(s);        // remove s
                    size--;    // subtract size by 1 because there is one less pair
                }
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// Do nothing if arguments are null or the first argument does not exist in the graph
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null || newDependents == null || new List<string>(newDependents).Count == 0)
            {
                return;
            }

            if (graph.ContainsKey(s) && graph[s][DEPENDENTS_INDEX].Count > 0)
            {
                List<string> dependentsofs = graph[s][DEPENDENTS_INDEX];

                while (dependentsofs.Count > 0) // trasverse through the dependents list of s 
                {
                    RemoveDependency(s, dependentsofs[0]);  // remove the relationship between s and the dependents list, and decrement the size. 
                }
            }

            // trasverse through the new dependents list
            foreach (string dependent in newDependents)
            {
                AddDependency(s, dependent); // add dependency relationship, and increment the size
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// Do nothing if arguments are null or the first argument does not exist in the graph
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (s == null || newDependees == null || new List<string>(newDependees).Count == 0)
            { // argument is either null or the size is 0
                return;
            }

            if (graph.ContainsKey(s) && graph[s][DEPENDEES_INDEX].Count > 0) // contains key and the dependee list contains stuff. 
            {
                List<string> dependeesofs = graph[s][DEPENDEES_INDEX];

                while (dependeesofs.Count > 0) // trasverse through the old dependees list, and remove each dependency individually. 
                {
                    RemoveDependency(dependeesofs[0], s);
                }
            }

            // trasverse through the newDependees list, and add s to its dependee list
            foreach (string dependee in newDependees)
            {
                AddDependency(dependee, s); // add dependency to both, and increment the size
            }
        }
    }
}
