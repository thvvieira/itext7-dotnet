/*

This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

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
namespace iText.IO.Font.Otf {
    public abstract class ContextualSubstRule {
        /// <returns>length of the context glyph sequence defined by this rule.</returns>
        public abstract int GetContextLength();

        /// <returns>
        /// an array of <code>SubstLookupRecord</code>. Each record specifies a position in the context glyph
        /// sequence and a LookupListIndex to the substitution lookup that is applied at that position.
        /// </returns>
        public abstract SubstLookupRecord[] GetSubstLookupRecords();

        /// <summary>Checks if glyph line element matches element from input sequence of the rule.</summary>
        /// <remarks>
        /// Checks if glyph line element matches element from input sequence of the rule.
        /// <br /><br />
        /// NOTE: rules do not contain the first element of the input sequence, the first element is defined by rule
        /// position in substitution table. Therefore atIdx shall not be 0.
        /// </remarks>
        /// <param name="glyphId"/>
        /// <param name="atIdx">index in the rule sequence. Shall be: 0 &lt; atIdx &lt; ContextualSubstRule.getContextLength().
        ///     </param>
        public abstract bool IsGlyphMatchesInput(int glyphId, int atIdx);

        /// <returns>length of the lookahead context glyph sequence defined by this rule.</returns>
        public virtual int GetLookaheadContextLength() {
            return 0;
        }

        /// <returns>length of the backtrack context glyph sequence defined by this rule.</returns>
        public virtual int GetBacktrackContextLength() {
            return 0;
        }

        /// <summary>Checks if glyph line element matches element from lookahead sequence of the rule.</summary>
        /// <param name="glyphId"/>
        /// <param name="atIdx">index in rule sequence. Shall be: 0 &lt;= atIdx &lt; ContextualSubstRule.getLookaheadContextLength().
        ///     </param>
        public virtual bool IsGlyphMatchesLookahead(int glyphId, int atIdx) {
            return false;
        }

        /// <summary>Checks if glyph line element matches element from backtrack sequence of the rule.</summary>
        /// <param name="glyphId"/>
        /// <param name="atIdx">index in rule sequence. Shall be: 0 &lt;= atIdx &lt; ContextualSubstRule.getBacktrackContextLength().
        ///     </param>
        public virtual bool IsGlyphMatchesBacktrack(int glyphId, int atIdx) {
            return false;
        }
    }
}
