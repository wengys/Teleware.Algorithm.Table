using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Teleware.Algorithm.TableBuilder.Shared.Tests
{
    public class CellReferenceTests
    {
        [Fact]
        public void EqualTest()
        {
            var a = CellReference.Create(0, 0);
            var b = CellReference.Create(0, 0);

            Assert.Equal(a, b);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void GreaterRowNumTest()
        {
            var a = CellReference.Create(1, 1);
            var b1 = CellReference.Create(2, 0);
            var b2 = CellReference.Create(2, 1);
            var b3 = CellReference.Create(2, 2);

            Assert.True(b1.CompareTo(a) > 0);
            Assert.True(b2.CompareTo(a) > 0);
            Assert.True(b3.CompareTo(a) > 0);
        }

        [Fact]
        public void GreaterColNumTest()
        {
            var a = CellReference.Create(1, 1);
            var b = CellReference.Create(1, 2);

            Assert.True(b.CompareTo(a) > 0);
        }
    }
}