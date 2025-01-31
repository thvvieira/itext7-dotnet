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
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Utils;
using iText.StyledXmlParser.Node;
using iText.StyledXmlParser.Node.Impl.Jsoup;
using iText.Svg.Exceptions;
using iText.Svg.Processors.Impl;
using iText.Svg.Renderers;
using iText.Test;

namespace iText.Svg.Renderers.Impl {
    public class PathSvgNodeRendererTest : SvgIntegrationTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/svg/renderers/impl/PathSvgNodeRendererTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/svg/renderers/impl/PathSvgNodeRendererTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            ITextTest.CreateDestinationFolder(destinationFolder);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathNodeRendererMoveToTest() {
            String filename = "pathNodeRendererMoveToTest.pdf";
            PdfDocument doc = new PdfDocument(new PdfWriter(destinationFolder + filename));
            doc.AddNewPage();
            IDictionary<String, String> pathShapes = new Dictionary<String, String>();
            pathShapes.Put("d", "M 100,100, L300,100,L200,300,z");
            ISvgNodeRenderer pathRenderer = new PathSvgNodeRenderer();
            pathRenderer.SetAttributesAndStyles(pathShapes);
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfCanvas cv = new PdfCanvas(doc, 1);
            context.PushCanvas(cv);
            pathRenderer.Draw(context);
            doc.Close();
            String result = new CompareTool().CompareByContent(destinationFolder + filename, sourceFolder + "cmp_" + filename
                , destinationFolder, "diff_");
            if (result != null && !result.Contains("No visual differences")) {
                NUnit.Framework.Assert.Fail(result);
            }
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathNodeRendererMoveToTest1() {
            //TODO (RND-904) This test should fail when RND-904 (relative line operator l ) is implemented.
            String filename = "pathNodeRendererMoveToTest1.pdf";
            PdfDocument doc = new PdfDocument(new PdfWriter(destinationFolder + filename));
            doc.AddNewPage();
            IDictionary<String, String> pathShapes = new Dictionary<String, String>();
            pathShapes.Put("d", "M 100 100 l300 100 L200 300 z");
            ISvgNodeRenderer pathRenderer = new PathSvgNodeRenderer();
            pathRenderer.SetAttributesAndStyles(pathShapes);
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfCanvas cv = new PdfCanvas(doc, 1);
            context.PushCanvas(cv);
            pathRenderer.Draw(context);
            doc.Close();
            String result = new CompareTool().CompareByContent(destinationFolder + filename, sourceFolder + "cmp_" + filename
                , destinationFolder, "diff_");
            if (result != null && !result.Contains("No visual differences")) {
                NUnit.Framework.Assert.Fail(result);
            }
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathNodeRendererCurveToTest() {
            String filename = "pathNodeRendererCurveToTest.pdf";
            PdfDocument doc = new PdfDocument(new PdfWriter(destinationFolder + filename));
            doc.AddNewPage();
            IDictionary<String, String> pathShapes = new Dictionary<String, String>();
            pathShapes.Put("d", "M100,200 C100,100 250,100 250,200 S400,300 400,200,z");
            ISvgNodeRenderer pathRenderer = new PathSvgNodeRenderer();
            pathRenderer.SetAttributesAndStyles(pathShapes);
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfCanvas cv = new PdfCanvas(doc, 1);
            context.PushCanvas(cv);
            pathRenderer.Draw(context);
            doc.Close();
            String result = new CompareTool().CompareByContent(destinationFolder + filename, sourceFolder + "cmp_" + filename
                , destinationFolder, "diff_");
            if (result != null && !result.Contains("No visual differences")) {
                NUnit.Framework.Assert.Fail(result);
            }
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathNodeRendererCurveToTest1() {
            String filename = "pathNodeRendererCurveToTest1.pdf";
            PdfDocument doc = new PdfDocument(new PdfWriter(destinationFolder + filename));
            doc.AddNewPage();
            IDictionary<String, String> pathShapes = new Dictionary<String, String>();
            pathShapes.Put("d", "M100 200 C100 300 250 300 250 200 S400 100 400 200 z");
            ISvgNodeRenderer pathRenderer = new PathSvgNodeRenderer();
            pathRenderer.SetAttributesAndStyles(pathShapes);
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfCanvas cv = new PdfCanvas(doc, 1);
            context.PushCanvas(cv);
            pathRenderer.Draw(context);
            doc.Close();
            String result = new CompareTool().CompareByContent(destinationFolder + filename, sourceFolder + "cmp_" + filename
                , destinationFolder, "diff_");
            if (result != null && !result.Contains("No visual differences")) {
                NUnit.Framework.Assert.Fail(result);
            }
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathNodeRendererQCurveToCurveToTest() {
            String filename = "pathNodeRendererQCurveToCurveToTest.pdf";
            PdfDocument doc = new PdfDocument(new PdfWriter(destinationFolder + filename));
            doc.AddNewPage();
            IDictionary<String, String> pathShapes = new Dictionary<String, String>();
            pathShapes.Put("d", "M200,300 Q400,50 600,300,z");
            ISvgNodeRenderer pathRenderer = new PathSvgNodeRenderer();
            pathRenderer.SetAttributesAndStyles(pathShapes);
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfCanvas cv = new PdfCanvas(doc, 1);
            context.PushCanvas(cv);
            pathRenderer.Draw(context);
            doc.Close();
            String result = new CompareTool().CompareByContent(destinationFolder + filename, sourceFolder + "cmp_" + filename
                , destinationFolder, "diff_");
            if (result != null && !result.Contains("No visual differences")) {
                NUnit.Framework.Assert.Fail(result);
            }
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathNodeRendererQCurveToCurveToTest1() {
            String filename = "pathNodeRendererQCurveToCurveToTest1.pdf";
            PdfDocument doc = new PdfDocument(new PdfWriter(destinationFolder + filename));
            doc.AddNewPage();
            IDictionary<String, String> pathShapes = new Dictionary<String, String>();
            pathShapes.Put("d", "M200 300 Q400 50 600 300 z");
            ISvgNodeRenderer pathRenderer = new PathSvgNodeRenderer();
            pathRenderer.SetAttributesAndStyles(pathShapes);
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfCanvas cv = new PdfCanvas(doc, 1);
            context.PushCanvas(cv);
            pathRenderer.Draw(context);
            doc.Close();
            String result = new CompareTool().CompareByContent(destinationFolder + filename, sourceFolder + "cmp_" + filename
                , destinationFolder, "diff_");
            if (result != null && !result.Contains("No visual differences")) {
                NUnit.Framework.Assert.Fail(result);
            }
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void SmoothCurveTest1() {
            String filename = "smoothCurveTest1.pdf";
            PdfDocument doc = new PdfDocument(new PdfWriter(destinationFolder + filename));
            doc.AddNewPage();
            String svgFilename = "smoothCurveTest1.svg";
            Stream xmlStream = new FileStream(sourceFolder + svgFilename, FileMode.Open, FileAccess.Read);
            IElementNode rootTag = new JsoupXmlParser().Parse(xmlStream, "ISO-8859-1");
            DefaultSvgProcessor processor = new DefaultSvgProcessor();
            IBranchSvgNodeRenderer root = (IBranchSvgNodeRenderer)processor.Process(rootTag).GetRootRenderer();
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfCanvas cv = new PdfCanvas(doc, 1);
            context.PushCanvas(cv);
            NUnit.Framework.Assert.IsTrue(root.GetChildren()[0] is PathSvgNodeRenderer);
            root.GetChildren()[0].Draw(context);
            doc.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void SmoothCurveTest2() {
            String filename = "smoothCurveTest2.pdf";
            PdfDocument doc = new PdfDocument(new PdfWriter(destinationFolder + filename));
            doc.AddNewPage();
            String svgFilename = "smoothCurveTest2.svg";
            Stream xmlStream = new FileStream(sourceFolder + svgFilename, FileMode.Open, FileAccess.Read);
            IElementNode rootTag = new JsoupXmlParser().Parse(xmlStream, "ISO-8859-1");
            DefaultSvgProcessor processor = new DefaultSvgProcessor();
            IBranchSvgNodeRenderer root = (IBranchSvgNodeRenderer)processor.Process(rootTag).GetRootRenderer();
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfCanvas cv = new PdfCanvas(doc, 1);
            context.PushCanvas(cv);
            NUnit.Framework.Assert.IsTrue(root.GetChildren()[0] is PathSvgNodeRenderer);
            root.GetChildren()[0].Draw(context);
            doc.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void SmoothCurveTest3() {
            String filename = "smoothCurveTest3.pdf";
            PdfDocument doc = new PdfDocument(new PdfWriter(destinationFolder + filename));
            doc.AddNewPage();
            String svgFilename = "smoothCurveTest3.svg";
            Stream xmlStream = new FileStream(sourceFolder + svgFilename, FileMode.Open, FileAccess.Read);
            IElementNode rootTag = new JsoupXmlParser().Parse(xmlStream, "ISO-8859-1");
            DefaultSvgProcessor processor = new DefaultSvgProcessor();
            IBranchSvgNodeRenderer root = (IBranchSvgNodeRenderer)processor.Process(rootTag).GetRootRenderer();
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfCanvas cv = new PdfCanvas(doc, 1);
            context.PushCanvas(cv);
            NUnit.Framework.Assert.IsTrue(root.GetChildren()[0] is PathSvgNodeRenderer);
            root.GetChildren()[0].Draw(context);
            doc.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathNodeRendererCurveComplexTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "curves");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathZOperatorTest01() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathZOperatorTest01");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathZOperatorTest02() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathZOperatorTest02");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathZOperatorTest03() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathZOperatorTest03");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathZOperatorTest04() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathZOperatorTest04");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void InvalidZOperatorTest01() {
            NUnit.Framework.Assert.That(() =>  {
                ConvertAndCompareVisually(sourceFolder, destinationFolder, "invalidZOperatorTest01");
            }
            , NUnit.Framework.Throws.InstanceOf<SvgProcessingException>())
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void InvalidOperatorTest01() {
            NUnit.Framework.Assert.That(() =>  {
                ConvertAndCompareVisually(sourceFolder, destinationFolder, "invalidOperatorTest01");
            }
            , NUnit.Framework.Throws.InstanceOf<SvgProcessingException>())
;
        }

        /* This test should fail when RND-1034 is resolved*/
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathLOperatorMultipleCoordinates() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathLOperatorMultipleCoordinates");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathVOperatorTest01() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathVOperatorTest01");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathZOperatorContinuePathingTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathZOperatorContinuePathingTest");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathVOperatorMultipleArgumentsTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathVOperatorMultipleArgumentsTest");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathHOperatorSimpleTest01() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathHOperatorSimpleTest01");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathHandVOperatorTest01() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathHandVOperatorTest01");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CurveToContinuePathingTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "curveToContinuePathingTest");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void RelativeHorizontalLineToTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "relativeHorizontalLineTo");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void RelativeVerticalLineToTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "relativeVerticalLineTo");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CombinedRelativeVerticalLineToAndRelativeHorizontalLineToTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "combinedRelativeVerticalLineToAndRelativeHorizontalLineTo"
                );
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MultipleRelativeHorizontalLineToTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "multipleRelativeHorizontalLineTo");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MultipleRelativeVerticalLineToTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "multipleRelativeVerticalLineTo");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MoveToRelativeMultipleTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "moveToRelativeMultiple");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MoveToAbsoluteMultipleTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "moveToAbsoluteMultiple");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ITextLogoTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "iTextLogo");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void EofillUnsuportedPathTest() {
            NUnit.Framework.Assert.That(() =>  {
                ConvertAndCompareVisually(sourceFolder, destinationFolder, "eofillUnsuportedPathTest");
            }
            , NUnit.Framework.Throws.InstanceOf<SvgProcessingException>())
;
        }
    }
}
