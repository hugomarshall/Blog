using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

public class TestPriorityOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
    {
        var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

        foreach (var testCase in testCases)
        {
            var priority = 0;

            foreach (var attr in testCase.TestMethod.Method.GetCustomAttributes((typeof(TestPriorityAttribute).AssemblyQualifiedName)))
            {
                priority = attr.GetNamedArgument<int>("Priority");
            }

            GetOrCreate(sortedMethods, priority).Add(testCase);
        }

        foreach (var list in sortedMethods.Keys.Select(priority => sortedMethods[priority]))
        {
            foreach (var testCase in list)
            {
                yield return testCase;
            }
        }
    }

    private static List<TTestCase> GetOrCreate<TTestCase>(IDictionary<int, List<TTestCase>> dictionary, int priority)
    {
        if (!dictionary.TryGetValue(priority, out var result))
        {
            result = new List<TTestCase>();
            dictionary[priority] = result;
        }

        return result;
    }
}
