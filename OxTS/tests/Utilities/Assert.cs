using Shouldly;
using System;
using System.Collections.Generic;

namespace OxTS.Tests
{
    public static class Assert
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
