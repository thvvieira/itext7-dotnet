/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Test;

namespace iText.Kernel.Pdf.Xobject {
    public class GetImageBytesTest : ExtendedITextTest {
        private static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/kernel/pdf/xobject/GetImageBytesTest/";

        private static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/kernel/pdf/xobject/GetImageBytesTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestMultiStageFilters() {
            // TODO DEVSIX-2940: extracted image is blank
            TestFile("multistagefilter1.pdf", "Obj13", "jpg");
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestAscii85Filters() {
            TestFile("ASCII85_RunLengthDecode.pdf", "Im9", "png");
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestCcittFilters() {
            TestFile("ccittfaxdecode.pdf", "background0", "png");
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestFlateDecodeFilters() {
            // TODO DEVSIX-2941: extracted indexed devicegray RunLengthDecode gets color inverted
            TestFile("flatedecode_runlengthdecode.pdf", "Im9", "png");
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestDctDecodeFilters() {
            // TODO DEVSIX-2940: extracted image is upside down
            TestFile("dctdecode.pdf", "im1", "jpg");
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void Testjbig2Filters() {
            // TODO DEVSIX-2942: extracted jbig2 image is not readable by most popular image viewers
            TestFile("jbig2decode.pdf", "2", "jbig2");
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [NUnit.Framework.Ignore("Ignored during the latest release. Please unignore after DEVSIX-3021")]
        public virtual void TestFlateCmyk() {
            TestFile("img_cmyk.pdf", "Im1", "tif");
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [NUnit.Framework.Ignore("Ignored during the latest release. Please unignore after DEVSIX-3021")]
        public virtual void TestFlateCmykIcc() {
            TestFile("img_cmyk_icc.pdf", "Im1", "tif");
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestFlateIndexed() {
            TestFile("img_indexed.pdf", "Im1", "png");
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestFlateRgbIcc() {
            TestFile("img_rgb_icc.pdf", "Im1", "png");
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestFlateRgb() {
            TestFile("img_rgb.pdf", "Im1", "png");
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestFlateCalRgb() {
            TestFile("img_calrgb.pdf", "Im1", "png");
        }

        /// <exception cref="System.Exception"/>
        private void TestFile(String filename, String objectid, String expectedImageFormat) {
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(sourceFolder + filename));
            try {
                PdfResources resources = pdfDocument.GetPage(1).GetResources();
                PdfDictionary xobjects = resources.GetResource(PdfName.XObject);
                PdfObject obj = xobjects.Get(new PdfName(objectid));
                if (obj == null) {
                    throw new ArgumentException("Reference " + objectid + " not found - Available keys are " + xobjects.KeySet
                        ());
                }
                PdfImageXObject img = new PdfImageXObject((PdfStream)(obj.IsIndirectReference() ? ((PdfIndirectReference)obj
                    ).GetRefersTo() : obj));
                NUnit.Framework.Assert.AreEqual(expectedImageFormat, img.IdentifyImageFileExtension());
                byte[] result = img.GetImageBytes(true);
                byte[] cmpBytes = File.ReadAllBytes(Path.Combine(sourceFolder, filename.JSubstring(0, filename.Length - 4)
                     + "." + expectedImageFormat));
                NUnit.Framework.Assert.AreEqual(cmpBytes, result);
            }
            finally {
                pdfDocument.Close();
            }
        }
    }
}
