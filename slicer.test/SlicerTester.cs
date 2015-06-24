using System;
using NUnit.Framework;

namespace slicer.test
{
    [TestFixture]
    public class SlicerTester
    {
        [Test]
        public void TestSlice()
        {
            var fileSlicer = new FileSlicer();
            fileSlicer.Slice(@"..\..\..\files\ExampleImage.jpg");

            fileSlicer.Reassemble();

        }
    }
}
