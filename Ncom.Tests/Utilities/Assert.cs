using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncom.Tests
{
    internal static class Assert
    {
        public static void AssertArraysAreEqual<T>(Span<T> actual, Span<T> expected, IEqualityComparer<T>? equalityComparer = null)
        {
            actual.Length.ShouldBe(expected.Length);

            for (int i = 0; i < actual.Length; i++)
            {
                actual[i].ShouldBe(expected[i], equalityComparer ?? EqualityComparer<T>.Default, $"At position {i}");
            }
        }
    }
}
