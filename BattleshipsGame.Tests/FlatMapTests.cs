using System;
using Xunit;


namespace BattleshipsGame.Tests
{
    public class FlatMapTests
    {
        [Theory]
        [InlineData("C4")]
        [InlineData("J9")]
        [InlineData("A0")]
        public void TestIsValidCoordinate(string position)
        {

            Assert.True(
                FlatMap.IsValidCoordinate(position).isValid, $"The coordinate {position} is valid"   
            );
        }
    }
}
