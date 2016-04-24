/*
$Id: 7243ab80d10a7a5ecbac984832337088d6751b10 $

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV
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
using System;
using System.Collections.Generic;
using com.itextpdf.kernel.geom;
using com.itextpdf.kernel.pdf;
using com.itextpdf.kernel.pdf.canvas;
using com.itextpdf.kernel.pdf.tagutils;
using com.itextpdf.layout;
using com.itextpdf.layout.element;
using com.itextpdf.layout.layout;

namespace com.itextpdf.layout.renderer
{
	public class BlockRenderer : AbstractRenderer
	{
		public BlockRenderer(BlockElement modelElement)
			: base(modelElement)
		{
		}

		public override LayoutResult Layout(LayoutContext layoutContext)
		{
			int pageNumber = layoutContext.GetArea().GetPageNumber();
			Rectangle parentBBox = layoutContext.GetArea().GetBBox().Clone();
			if (GetProperty(Property.ROTATION_ANGLE) != null)
			{
				parentBBox.MoveDown(AbstractRenderer.INF - parentBBox.GetHeight()).SetHeight(AbstractRenderer
					.INF);
			}
			float blockHeight = RetrieveHeight();
			if (!IsFixedLayout() && blockHeight != null && blockHeight > parentBBox.GetHeight
				())
			{
				return new LayoutResult(LayoutResult.NOTHING, null, null, this);
			}
			ApplyMargins(parentBBox, false);
			ApplyBorderBox(parentBBox, false);
			if (IsPositioned())
			{
				float x = GetPropertyAsFloat(Property.X);
				float relativeX = IsFixedLayout() ? 0 : parentBBox.GetX();
				parentBBox.SetX(relativeX + x);
			}
			float blockWidth = RetrieveWidth(parentBBox.GetWidth());
			if (blockWidth != null && (blockWidth < parentBBox.GetWidth() || IsPositioned()))
			{
				parentBBox.SetWidth(blockWidth);
			}
			ApplyPaddings(parentBBox, false);
			IList<Rectangle> areas;
			if (IsPositioned())
			{
				areas = java.util.Collections.SingletonList(parentBBox);
			}
			else
			{
				areas = InitElementAreas(new LayoutArea(pageNumber, parentBBox));
			}
			occupiedArea = new LayoutArea(pageNumber, new Rectangle(parentBBox.GetX(), parentBBox
				.GetY() + parentBBox.GetHeight(), parentBBox.GetWidth(), 0));
			int currentAreaPos = 0;
			Rectangle layoutBox = areas[0].Clone();
			bool anythingPlaced = false;
			for (int childPos = 0; childPos < childRenderers.Count; childPos++)
			{
				IRenderer childRenderer = childRenderers[childPos];
				LayoutResult result;
				while ((result = childRenderer.Layout(new LayoutContext(new LayoutArea(pageNumber
					, layoutBox)))).GetStatus() != LayoutResult.FULL)
				{
					if (result.GetOccupiedArea() != null)
					{
						occupiedArea.SetBBox(Rectangle.GetCommonRectangle(occupiedArea.GetBBox(), result.
							GetOccupiedArea().GetBBox()));
						layoutBox.SetHeight(layoutBox.GetHeight() - result.GetOccupiedArea().GetBBox().GetHeight
							());
					}
					if (childRenderer.GetProperty(Property.WIDTH) != null)
					{
						AlignChildHorizontally(childRenderer, layoutBox.GetWidth());
					}
					// have more areas
					if (currentAreaPos + 1 < areas.Count)
					{
						if (result.GetStatus() == LayoutResult.PARTIAL)
						{
							childRenderers[childPos] = result.GetSplitRenderer();
							// TODO linkedList would make it faster
							childRenderers.Insert(childPos + 1, result.GetOverflowRenderer());
						}
						else
						{
							childRenderers[childPos] = result.GetOverflowRenderer();
							childPos--;
						}
						layoutBox = areas[++currentAreaPos].Clone();
						break;
					}
					else
					{
						if (result.GetStatus() == LayoutResult.PARTIAL)
						{
							layoutBox.SetHeight(layoutBox.GetHeight() - result.GetOccupiedArea().GetBBox().GetHeight
								());
							if (currentAreaPos + 1 == areas.Count)
							{
								com.itextpdf.layout.renderer.BlockRenderer splitRenderer = CreateSplitRenderer(LayoutResult
									.PARTIAL);
								splitRenderer.childRenderers = new List<IRenderer>(childRenderers.SubList(0, childPos
									));
								splitRenderer.childRenderers.Add(result.GetSplitRenderer());
								splitRenderer.occupiedArea = occupiedArea;
								com.itextpdf.layout.renderer.BlockRenderer overflowRenderer = CreateOverflowRenderer
									(LayoutResult.PARTIAL);
								IList<IRenderer> overflowRendererChildren = new List<IRenderer>();
								overflowRendererChildren.Add(result.GetOverflowRenderer());
								overflowRendererChildren.AddAll(childRenderers.SubList(childPos + 1, childRenderers
									.Count));
								overflowRenderer.childRenderers = overflowRendererChildren;
								ApplyPaddings(occupiedArea.GetBBox(), true);
								ApplyBorderBox(occupiedArea.GetBBox(), true);
								ApplyMargins(occupiedArea.GetBBox(), true);
								return new LayoutResult(LayoutResult.PARTIAL, occupiedArea, splitRenderer, overflowRenderer
									);
							}
							else
							{
								childRenderers[childPos] = result.GetSplitRenderer();
								childRenderers.Insert(childPos + 1, result.GetOverflowRenderer());
								layoutBox = areas[++currentAreaPos].Clone();
								break;
							}
						}
						else
						{
							if (result.GetStatus() == LayoutResult.NOTHING)
							{
								bool keepTogether = GetPropertyAsBoolean(Property.KEEP_TOGETHER);
								int layoutResult = anythingPlaced && !keepTogether ? LayoutResult.PARTIAL : LayoutResult
									.NOTHING;
								com.itextpdf.layout.renderer.BlockRenderer splitRenderer = CreateSplitRenderer(layoutResult
									);
								splitRenderer.childRenderers = new List<IRenderer>(childRenderers.SubList(0, childPos
									));
								com.itextpdf.layout.renderer.BlockRenderer overflowRenderer = CreateOverflowRenderer
									(layoutResult);
								IList<IRenderer> overflowRendererChildren = new List<IRenderer>();
								overflowRendererChildren.Add(result.GetOverflowRenderer());
								overflowRendererChildren.AddAll(childRenderers.SubList(childPos + 1, childRenderers
									.Count));
								foreach (IRenderer renderer in overflowRendererChildren)
								{
									renderer.SetParent(overflowRenderer);
								}
								overflowRenderer.childRenderers = overflowRendererChildren;
								if (keepTogether)
								{
									splitRenderer = null;
									overflowRenderer.childRenderers.Clear();
									overflowRenderer.childRenderers = new List<IRenderer>(childRenderers);
								}
								ApplyPaddings(occupiedArea.GetBBox(), true);
								ApplyBorderBox(occupiedArea.GetBBox(), true);
								ApplyMargins(occupiedArea.GetBBox(), true);
								if (GetPropertyAsBoolean(Property.FORCED_PLACEMENT))
								{
									return new LayoutResult(LayoutResult.FULL, occupiedArea, null, null);
								}
								else
								{
									return new LayoutResult(layoutResult, occupiedArea, splitRenderer, overflowRenderer
										);
								}
							}
						}
					}
				}
				anythingPlaced = true;
				occupiedArea.SetBBox(Rectangle.GetCommonRectangle(occupiedArea.GetBBox(), result.
					GetOccupiedArea().GetBBox()));
				if (result.GetStatus() == LayoutResult.FULL)
				{
					layoutBox.SetHeight(layoutBox.GetHeight() - result.GetOccupiedArea().GetBBox().GetHeight
						());
					if (childRenderer.GetProperty(Property.WIDTH) != null)
					{
						AlignChildHorizontally(childRenderer, layoutBox.GetWidth());
					}
				}
			}
			ApplyPaddings(occupiedArea.GetBBox(), true);
			if (blockHeight != null && blockHeight > occupiedArea.GetBBox().GetHeight())
			{
				occupiedArea.GetBBox().MoveDown(blockHeight - occupiedArea.GetBBox().GetHeight())
					.SetHeight(blockHeight);
			}
			if (IsPositioned())
			{
				float y = GetPropertyAsFloat(Property.Y);
				float relativeY = IsFixedLayout() ? 0 : layoutBox.GetY();
				Move(0, relativeY + y - occupiedArea.GetBBox().GetY());
			}
			ApplyBorderBox(occupiedArea.GetBBox(), true);
			ApplyMargins(occupiedArea.GetBBox(), true);
			if (GetProperty(Property.ROTATION_ANGLE) != null)
			{
				ApplyRotationLayout(layoutContext.GetArea().GetBBox().Clone());
				if (IsNotFittingHeight(layoutContext.GetArea()))
				{
					if (!GetPropertyAsBoolean(Property.FORCED_PLACEMENT))
					{
						return new LayoutResult(LayoutResult.NOTHING, occupiedArea, null, this);
					}
				}
			}
			ApplyVerticalAlignment();
			return new LayoutResult(LayoutResult.FULL, occupiedArea, null, null);
		}

		public override IRenderer GetNextRenderer()
		{
			return new com.itextpdf.layout.renderer.BlockRenderer((BlockElement)modelElement);
		}

		protected internal virtual com.itextpdf.layout.renderer.BlockRenderer CreateSplitRenderer
			(int layoutResult)
		{
			com.itextpdf.layout.renderer.BlockRenderer splitRenderer = ((com.itextpdf.layout.renderer.BlockRenderer
				)GetNextRenderer());
			splitRenderer.parent = parent;
			splitRenderer.modelElement = modelElement;
			splitRenderer.occupiedArea = occupiedArea;
			splitRenderer.isLastRendererForModelElement = false;
			return splitRenderer;
		}

		protected internal virtual com.itextpdf.layout.renderer.BlockRenderer CreateOverflowRenderer
			(int layoutResult)
		{
			com.itextpdf.layout.renderer.BlockRenderer overflowRenderer = ((com.itextpdf.layout.renderer.BlockRenderer
				)GetNextRenderer());
			overflowRenderer.parent = parent;
			overflowRenderer.modelElement = modelElement;
			overflowRenderer.properties = properties;
			return overflowRenderer;
		}

		public override void Draw(DrawContext drawContext)
		{
			PdfDocument document = drawContext.GetDocument();
			ApplyDestination(document);
			ApplyAction(document);
			bool isTagged = drawContext.IsTaggingEnabled() && GetModelElement() is IAccessibleElement;
			TagTreePointer tagPointer = null;
			IAccessibleElement accessibleElement = null;
			if (isTagged)
			{
				accessibleElement = (IAccessibleElement)GetModelElement();
				PdfName role = accessibleElement.GetRole();
				if (role != null && !PdfName.Artifact.Equals(role))
				{
					tagPointer = document.GetTagStructureContext().GetAutoTaggingPointer();
					if (!tagPointer.IsElementConnectedToTag(accessibleElement))
					{
						AccessibleAttributesApplier.ApplyLayoutAttributes(role, this, document);
						if (role.Equals(PdfName.TD))
						{
							AccessibleAttributesApplier.ApplyTableAttributes(this);
						}
						if (role.Equals(PdfName.List))
						{
							AccessibleAttributesApplier.ApplyListAttributes(this);
						}
					}
					tagPointer.AddTag(accessibleElement, true);
				}
				else
				{
					isTagged = false;
				}
			}
			int position = GetPropertyAsInteger(Property.POSITION);
			if (position == LayoutPosition.RELATIVE)
			{
				ApplyAbsolutePositioningTranslation(false);
			}
			BeginRotationIfApplied(drawContext.GetCanvas());
			DrawBackground(drawContext);
			DrawBorder(drawContext);
			DrawChildren(drawContext);
			EndRotationIfApplied(drawContext.GetCanvas());
			if (position == LayoutPosition.RELATIVE)
			{
				ApplyAbsolutePositioningTranslation(true);
			}
			if (isTagged)
			{
				tagPointer.MoveToParent();
				if (isLastRendererForModelElement)
				{
					document.GetTagStructureContext().RemoveElementConnectionToTag(accessibleElement);
				}
			}
			flushed = true;
		}

		public override Rectangle GetOccupiedAreaBBox()
		{
			Rectangle bBox = occupiedArea.GetBBox().Clone();
			float rotationAngle = GetProperty(Property.ROTATION_ANGLE);
			if (rotationAngle != null)
			{
				bBox.SetWidth(GetPropertyAsFloat(Property.ROTATION_INITIAL_WIDTH));
				bBox.SetHeight(GetPropertyAsFloat(Property.ROTATION_INITIAL_HEIGHT));
			}
			return bBox;
		}

		protected internal virtual void ApplyVerticalAlignment()
		{
			Property.VerticalAlignment verticalAlignment = GetProperty(Property.VERTICAL_ALIGNMENT
				);
			if (verticalAlignment != null && verticalAlignment != Property.VerticalAlignment.
				TOP && childRenderers.Count > 0)
			{
				float deltaY = childRenderers[childRenderers.Count - 1].GetOccupiedArea().GetBBox
					().GetY() - GetInnerAreaBBox().GetY();
				switch (verticalAlignment)
				{
					case Property.VerticalAlignment.BOTTOM:
					{
						foreach (IRenderer child in childRenderers)
						{
							child.Move(0, -deltaY);
						}
						break;
					}

					case Property.VerticalAlignment.MIDDLE:
					{
						foreach (IRenderer child_1 in childRenderers)
						{
							child_1.Move(0, -deltaY / 2);
						}
						break;
					}
				}
			}
		}

		protected internal virtual void ApplyRotationLayout(Rectangle layoutBox)
		{
			float rotationPointX = GetPropertyAsFloat(Property.ROTATION_POINT_X);
			float rotationPointY = GetPropertyAsFloat(Property.ROTATION_POINT_Y);
			if (rotationPointX == null || rotationPointY == null)
			{
				// if rotation point was not specified, the most bottom-left point is used
				rotationPointX = occupiedArea.GetBBox().GetX();
				rotationPointY = occupiedArea.GetBBox().GetY();
				SetProperty(Property.ROTATION_POINT_X, rotationPointX);
				SetProperty(Property.ROTATION_POINT_Y, rotationPointY);
			}
			float height = occupiedArea.GetBBox().GetHeight();
			float width = occupiedArea.GetBBox().GetWidth();
			SetProperty(Property.ROTATION_INITIAL_WIDTH, width);
			SetProperty(Property.ROTATION_INITIAL_HEIGHT, height);
			if (!IsPositioned())
			{
				IList<Point> rotatedPoints = new List<Point>();
				GetLayoutShiftAndRotatedPoints(rotatedPoints, rotationPointX, rotationPointY);
				Point clipLineBeg = new Point(layoutBox.GetRight(), layoutBox.GetTop());
				Point clipLineEnd = new Point(layoutBox.GetRight(), layoutBox.GetBottom());
				IList<Point> newOccupiedBox = ClipBBox(rotatedPoints, clipLineBeg, clipLineEnd);
				double maxX = -double.MaxValue;
				double minY = double.MaxValue;
				foreach (Point point in newOccupiedBox)
				{
					if (point.GetX() > maxX)
					{
						maxX = point.GetX();
					}
					if (point.GetY() < minY)
					{
						minY = point.GetY();
					}
				}
				float newHeight = (float)(occupiedArea.GetBBox().GetTop() - minY);
				float newWidth = (float)(maxX - occupiedArea.GetBBox().GetLeft());
				occupiedArea.GetBBox().SetWidth(newWidth);
				occupiedArea.GetBBox().SetHeight(newHeight);
				Move(0, height - newHeight);
			}
		}

		protected internal virtual float[] ApplyRotation()
		{
			float dx = 0;
			float dy = 0;
			if (!IsPositioned())
			{
				Point shift = GetLayoutShiftAndRotatedPoints(new List<Point>(), 0, 0);
				dy = (float)shift.GetY();
				dx = (float)shift.GetX();
			}
			float angle = GetPropertyAsFloat(Property.ROTATION_ANGLE);
			AffineTransform transform = new AffineTransform();
			transform.Rotate(angle);
			float[] ctm = new float[6];
			transform.GetMatrix(ctm);
			ctm[4] = GetPropertyAsFloat(Property.ROTATION_POINT_X) + dx;
			ctm[5] = GetPropertyAsFloat(Property.ROTATION_POINT_Y) + dy;
			return ctm;
		}

		private Point GetLayoutShiftAndRotatedPoints(IList<Point> rotatedPoints, float shiftX
			, float shiftY)
		{
			float angle = GetPropertyAsFloat(Property.ROTATION_ANGLE);
			float width = GetPropertyAsFloat(Property.ROTATION_INITIAL_WIDTH);
			float height = GetPropertyAsFloat(Property.ROTATION_INITIAL_HEIGHT);
			float left = occupiedArea.GetBBox().GetX() - shiftX;
			float bottom = occupiedArea.GetBBox().GetY() - shiftY;
			float right = left + width;
			float top = bottom + height;
			AffineTransform rotateTransform = new AffineTransform();
			rotateTransform.Rotate(angle);
			TransformBBox(left, bottom, right, top, rotateTransform, rotatedPoints);
			double minX = double.MaxValue;
			double maxY = -double.MaxValue;
			foreach (Point point in rotatedPoints)
			{
				if (point.GetX() < minX)
				{
					minX = point.GetX();
				}
				if (point.GetY() > maxY)
				{
					maxY = point.GetY();
				}
			}
			float dx = (float)(left - minX);
			float dy = (float)(top - maxY);
			foreach (Point point_1 in rotatedPoints)
			{
				point_1.SetLocation(point_1.GetX() + dx + shiftX, point_1.GetY() + dy + shiftY);
			}
			return new Point(dx, dy);
		}

		protected internal virtual void BeginRotationIfApplied(PdfCanvas canvas)
		{
			float angle = GetPropertyAsFloat(Property.ROTATION_ANGLE);
			if (angle != null)
			{
				float heightDiff = GetPropertyAsFloat(Property.ROTATION_INITIAL_HEIGHT) - occupiedArea
					.GetBBox().GetHeight();
				float shiftX = GetPropertyAsFloat(Property.ROTATION_POINT_X);
				float shiftY = GetPropertyAsFloat(Property.ROTATION_POINT_Y) + heightDiff;
				Move(-shiftX, -shiftY);
				float[] ctm = ApplyRotation();
				canvas.SaveState().ConcatMatrix(ctm[0], ctm[1], ctm[2], ctm[3], ctm[4], ctm[5]);
			}
		}

		protected internal virtual void EndRotationIfApplied(PdfCanvas canvas)
		{
			float angle = GetPropertyAsFloat(Property.ROTATION_ANGLE);
			if (angle != null)
			{
				float heightDiff = GetPropertyAsFloat(Property.ROTATION_INITIAL_HEIGHT) - occupiedArea
					.GetBBox().GetHeight();
				float shiftX = GetPropertyAsFloat(Property.ROTATION_POINT_X);
				float shiftY = GetPropertyAsFloat(Property.ROTATION_POINT_Y) + heightDiff;
				canvas.RestoreState();
				Move(shiftX, shiftY);
			}
		}

		private IList<Point> TransformBBox(float left, float bottom, float right, float top
			, AffineTransform transform, IList<Point> bBoxPoints)
		{
			bBoxPoints.AddAll(com.itextpdf.io.util.JavaUtil.ArraysAsList(new Point(left, bottom
				), new Point(right, bottom), new Point(right, top), new Point(left, top)));
			foreach (Point point in bBoxPoints)
			{
				transform.Transform(point, point);
			}
			return bBoxPoints;
		}

		private IList<Point> ClipBBox(IList<Point> points, Point clipLineBeg, Point clipLineEnd
			)
		{
			IList<Point> filteredPoints = new List<Point>();
			bool prevOnRightSide = false;
			Point filteringPoint = points[0];
			if (CheckPointSide(filteringPoint, clipLineBeg, clipLineEnd) >= 0)
			{
				filteredPoints.Add(filteringPoint);
				prevOnRightSide = true;
			}
			Point prevPoint = filteringPoint;
			for (int i = 1; i < points.Count + 1; ++i)
			{
				filteringPoint = points[i % points.Count];
				if (CheckPointSide(filteringPoint, clipLineBeg, clipLineEnd) >= 0)
				{
					if (!prevOnRightSide)
					{
						filteredPoints.Add(GetIntersectionPoint(prevPoint, filteringPoint, clipLineBeg, clipLineEnd
							));
					}
					filteredPoints.Add(filteringPoint);
					prevOnRightSide = true;
				}
				else
				{
					if (prevOnRightSide)
					{
						filteredPoints.Add(GetIntersectionPoint(prevPoint, filteringPoint, clipLineBeg, clipLineEnd
							));
					}
				}
				prevPoint = filteringPoint;
			}
			return filteredPoints;
		}

		private int CheckPointSide(Point filteredPoint, Point clipLineBeg, Point clipLineEnd
			)
		{
			double x1;
			double x2;
			double y1;
			double y2;
			x1 = filteredPoint.GetX() - clipLineBeg.GetX();
			y2 = clipLineEnd.GetY() - clipLineBeg.GetY();
			x2 = clipLineEnd.GetX() - clipLineBeg.GetX();
			y1 = filteredPoint.GetY() - clipLineBeg.GetY();
			double sgn = x1 * y2 - x2 * y1;
			if (Math.Abs(sgn) < 0.001)
			{
				return 0;
			}
			if (sgn > 0)
			{
				return 1;
			}
			if (sgn < 0)
			{
				return -1;
			}
			return 0;
		}

		private Point GetIntersectionPoint(Point lineBeg, Point lineEnd, Point clipLineBeg
			, Point clipLineEnd)
		{
			double A1 = lineBeg.GetY() - lineEnd.GetY();
			double A2 = clipLineBeg.GetY() - clipLineEnd.GetY();
			double B1 = lineEnd.GetX() - lineBeg.GetX();
			double B2 = clipLineEnd.GetX() - clipLineBeg.GetX();
			double C1 = lineBeg.GetX() * lineEnd.GetY() - lineBeg.GetY() * lineEnd.GetX();
			double C2 = clipLineBeg.GetX() * clipLineEnd.GetY() - clipLineBeg.GetY() * clipLineEnd
				.GetX();
			double M = B1 * A2 - B2 * A1;
			return new Point((B2 * C1 - B1 * C2) / M, (C2 * A1 - C1 * A2) / M);
		}
	}
}
