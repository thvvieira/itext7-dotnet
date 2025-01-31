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
using iText.Kernel.Pdf.Canvas;
using iText.StyledXmlParser.Css.Util;
using iText.Svg;
using iText.Svg.Renderers;
using iText.Svg.Utils;

namespace iText.Svg.Renderers.Impl {
    /// <summary>
    /// <see cref="iText.Svg.Renderers.ISvgNodeRenderer"/>
    /// implementation for the &lt;circle&gt; tag.
    /// </summary>
    public class EllipseSvgNodeRenderer : AbstractSvgNodeRenderer {
        internal float cx;

        internal float cy;

        internal float rx;

        internal float ry;

        protected internal override void DoDraw(SvgDrawContext context) {
            PdfCanvas cv = context.GetCurrentCanvas();
            cv.WriteLiteral("% ellipse\n");
            if (SetParameters()) {
                cv.MoveTo(cx + rx, cy);
                DrawUtils.Arc(cx - rx, cy - ry, cx + rx, cy + ry, 0, 360, cv);
            }
        }

        /// <summary>
        /// Fetches a map of String values by calling getAttribute(Strng s) method
        /// and maps it's values to arc parmateter cx, cy , rx, ry respectively
        /// </summary>
        /// <returns>boolean values to indicate whether all values exit or not</returns>
        protected internal virtual bool SetParameters() {
            cx = 0;
            cy = 0;
            if (GetAttribute(SvgConstants.Attributes.CX) != null) {
                cx = CssUtils.ParseAbsoluteLength(GetAttribute(SvgConstants.Attributes.CX));
            }
            if (GetAttribute(SvgConstants.Attributes.CY) != null) {
                cy = CssUtils.ParseAbsoluteLength(GetAttribute(SvgConstants.Attributes.CY));
            }
            if (GetAttribute(SvgConstants.Attributes.RX) != null && CssUtils.ParseAbsoluteLength(GetAttribute(SvgConstants.Attributes
                .RX)) > 0) {
                rx = CssUtils.ParseAbsoluteLength(GetAttribute(SvgConstants.Attributes.RX));
            }
            else {
                return false;
            }
            //No drawing if rx is absent
            if (GetAttribute(SvgConstants.Attributes.RY) != null && CssUtils.ParseAbsoluteLength(GetAttribute(SvgConstants.Attributes
                .RY)) > 0) {
                ry = CssUtils.ParseAbsoluteLength(GetAttribute(SvgConstants.Attributes.RY));
            }
            else {
                return false;
            }
            //No drawing if ry is absent
            return true;
        }

        public override ISvgNodeRenderer CreateDeepCopy() {
            EllipseSvgNodeRenderer copy = new EllipseSvgNodeRenderer();
            DeepCopyAttributesAndStyles(copy);
            return copy;
        }
    }
}
