using System;
using Xunit;


namespace BattleshipsGame.Tests
{
    public class FlatMapTests
    {
        [Theory]
        [InlineData("c4")]
        [InlineData("j9")]
        [InlineData("a0")]
        public void TestIsValidCoordinate(string position)
        {

            Assert.True(
                FlatMap.IsValidCoordinate(position).isValid, $"The coordinate {position} is valid"   
            );
        }
    }
}
