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
using iText.Test;

namespace iText.Svg.Renderers {
    public class OpacityTest : SvgIntegrationTest {
        private static readonly String SOURCE_FOLDER = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/svg/renderers/impl/OpacityTest/";

        private static readonly String DESTINATION_FOLDER = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/svg/renderers/impl/OpacityTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            ITextTest.CreateDestinationFolder(DESTINATION_FOLDER);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestOpacitySimple() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "opacity_simple");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestOpacityRGBA() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "opacity_rgba");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestOpacityComplex() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "opacity_complex");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestRGBA() {
            //TODO: update after DEVSIX-2673 fix
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "svg_rgba");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestFillOpacityWithComma() {
            NUnit.Framework.Assert.That(() =>  {
                //TODO DEVSIX-2678
                ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "testFillOpacityWithComma");
            }
            , NUnit.Framework.Throws.InstanceOf<FormatException>())
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestFillOpacityWithPercents() {
            NUnit.Framework.Assert.That(() =>  {
                //TODO DEVSIX-2678
                ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "testFillOpacityWithPercents");
            }
            , NUnit.Framework.Throws.InstanceOf<FormatException>())
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestFillOpacity() {
            //TODO: update after DEVSIX-2678 fix
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "svg_fill_opacity");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestStrokeOpacityWithComma() {
            NUnit.Framework.Assert.That(() =>  {
                //TODO DEVSIX-2679
                ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "testStrokeOpacityWithComma");
            }
            , NUnit.Framework.Throws.InstanceOf<Exception>())
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestStrokeOpacityWithPercents() {
            NUnit.Framework.Assert.That(() =>  {
                //TODO DEVSIX-2679
                ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "testStrokeOpacityWithPercents");
            }
            , NUnit.Framework.Throws.InstanceOf<FormatException>())
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void TestStrokeOpacity() {
            //TODO: update after DEVSIX-2679 fix
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "svg_stroke_opacity");
        }
    }
}
