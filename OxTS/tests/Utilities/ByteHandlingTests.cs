using Shouldly;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace OxTS.Tests
{
    public class ByteHandlingTests
    {
        #region Marshal

        [Theory]
        [InlineData(22013, "FD 55")]
        public void Test_marshal_short(ushort value, string expectedHex)
        {
            // Arrange
            byte[] expected = ConvertHexStringToByteArray(expectedHex);
            byte[] buffer = new byte[2];

            // Act
            ByteHandling.Marshal(buffer.AsSpan(), value);

            // Assert
            Assert.AssertArraysAreEqual(buffer.AsSpan(), expected.AsSpan());
        }

        [Theory]
        [InlineData(2842,   "1A 0B 00")]
        [InlineData(1182,   "9E 04 00")]
        [InlineData(-96331, "B5 87 FE")]
        [InlineData(-387,   "7D FE FF")]
        [InlineData(145,    "91 00 00")]
        [InlineData(-565,   "CB FD FF")]
        public void Test_marshal_int24(int value, string expectedHex)
        {
            // Arrange
            byte[] expected = ConvertHexStringToByteArray(expectedHex);
            byte[] buffer = new byte[4];

            // Act
            ByteHandling.Marshal(buffer.AsSpan(), value);

            // Assert
            Assert.AssertArraysAreEqual(buffer.AsSpan(0, 3), expected.AsSpan());
        }

        [Theory]
        [InlineData(1.2469839, "2B 9D 9F 3F")]
        //[InlineData(1.24698, "2B 9D 9F 3F")]
        public void Test_marshal_float(float value, string expectedHex)
        {
            // Arrange
            byte[] expected = ConvertHexStringToByteArray(expectedHex);
            byte[] buffer = new byte[4];

            // Act
            ByteHandling.Marshal(buffer.AsSpan(), value);

            // Assert
            Assert.AssertArraysAreEqual(buffer.AsSpan(), expected.AsSpan());
        }

        [Theory]
        [InlineData( 0.6542659401551972, "F3 4D FB 1F BF EF E4 3F")]
        [InlineData(-2.133108603807231,  "A2 58 61 3E 9B 10 01 C0")]
        public void Test_marshal_double(double value, string expectedHex)
        {
            // Arrange
            byte[] expected = ConvertHexStringToByteArray(expectedHex);
            byte[] buffer = new byte[8];

            // Act
            ByteHandling.Marshal(buffer.AsSpan(), value);

            // Assert
            Assert.AssertArraysAreEqual(buffer.AsSpan(), expected.AsSpan());
        }

        #endregion

        #region Unmarshal

        [Theory]
        [InlineData("FD 55", 22013)]
        public void Test_unmarshal_short(string hex, ushort expectedValue)
        {
            // Arrange
            byte[] buffer = ConvertHexStringToByteArray(hex);

            // Act
            ByteHandling.Unmarshal(buffer.AsSpan(), out ushort value);

            // Assert
            value.ShouldBe(expectedValue);
        }

        [Theory]
        [InlineData("1A 0B 00",  2842)]
        [InlineData("9E 04 00",  1182)]
        [InlineData("B5 87 FE", -96331)]
        [InlineData("7D FE FF", -387)]
        [InlineData("91 00 00",  145)]
        [InlineData("CB FD FF", -565)]
        public void Test_unmarshal_int24(string hex, int expectedValue)
        {
            // Arrange
            // Append 0xFF to the hex string, to ensure that unmarshal ignores the 4th byte when unmarshalling
            byte[] buffer = ConvertHexStringToByteArray(hex + "4A");

            // Act
            ByteHandling.UnmarshalInt24(buffer.AsSpan(), out int value);

            // Assert
            value.ShouldBe(expectedValue);
        }

        [Theory]
        [InlineData("2B 9D 9F 3F", 1.2469839)]
        public void Test_unmarshal_float(string hex, float expectedValue)
        {
            // Arrange
            byte[] buffer = ConvertHexStringToByteArray(hex);

            // Act
            ByteHandling.Unmarshal(buffer.AsSpan(), out float value);

            // Assert
            value.ShouldBe(expectedValue);
        }

        [Theory]
        [InlineData("F3 4D FB 1F BF EF E4 3F",  0.6542659401551972)]
        [InlineData("A2 58 61 3E 9B 10 01 C0", -2.133108603807231)]
        public void Test_unmarshal_double(string hex, double expectedValue)
        {
            // Arrange
            byte[] buffer = ConvertHexStringToByteArray(hex);

            // Act
            ByteHandling.Unmarshal(buffer.AsSpan(), out double value);

            // Assert
            value.ShouldBe(expectedValue);
        }

        #endregion


        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "");

            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }
    }
}
