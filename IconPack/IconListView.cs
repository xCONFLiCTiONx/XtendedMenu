using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TAFactory.IconPack
{
    public partial class IconListView : ListView
    {
        private const int minWidth = 64;
        private const int textHeight = 18;
        private const int verticalSpacing = 5;
        private static readonly Padding TilePadding = new Padding(5, 1, 5, 1);

        public IconListView()
        {
            InitializeComponent();
            base.View = View.Tile;
            TileSize = base.TileSize;
            base.OwnerDraw = true;
            base.DrawItem += new DrawListViewItemEventHandler(IconListView_DrawItem);
        }

        private Size _tileSize;
        public new Size TileSize
        {
            get => _tileSize;
            set
            {
                _tileSize = value;
                base.BeginUpdate();
                base.TileSize = new Size(Math.Max(minWidth, value.Width) + TilePadding.Horizontal, value.Height + verticalSpacing + textHeight + TilePadding.Vertical);
                if (base.Items.Count != 0)
                {
                    List<IconListViewItem> list = new List<IconListViewItem>(base.Items.Count);
                    foreach (IconListViewItem item in base.Items)
                    {
                        list.Add(item);
                    }
                    base.Items.Clear();
                    foreach (IconListViewItem item in list)
                    {
                        base.Items.Add(item);
                    }
                    //base.RedrawItems(0, base.Items.Count - 1, false);
                }

                base.EndUpdate();
            }
        }

        private void IconListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (!(e.Item is IconListViewItem item))
            {
                e.DrawDefault = true;
                return;
            }

            // Draw item
            e.DrawBackground();
            Pen border = SystemPens.ControlLight;
            if (e.Item.Selected)
            {
                if (Focused)
                {
                    border = SystemPens.Highlight;
                }
                else
                {
                    border = SystemPens.ButtonFace;
                }
            }
            int centerSpacing = (e.Bounds.Width - TileSize.Width - TilePadding.Horizontal) / 2 + TilePadding.Left;
            Rectangle newBounds = new Rectangle(e.Bounds.X + centerSpacing, e.Bounds.Y + TilePadding.Top, TileSize.Width, TileSize.Height);
            e.Graphics.DrawRectangle(border, newBounds);

            //e.Graphics.DrawString("Whatever", this.Font, e., 0, 0);
            int x = e.Bounds.X + (newBounds.Width - item.Icon.Width) / 2 + centerSpacing + 1;
            int y = e.Bounds.Y + (newBounds.Height - item.Icon.Height) / 2 + TilePadding.Top + 1;
            Rectangle rect = new Rectangle(x, y, item.Icon.Width, item.Icon.Height);
            Region clipReg = new Region(newBounds);
            e.Graphics.Clip = clipReg;
            e.Graphics.DrawIcon(item.Icon, rect);

            string text = string.Format("{0} x {1}", item.Icon.Width, item.Icon.Height);
            SizeF stringSize = e.Graphics.MeasureString(text, Font);
            int stringWidth = (int)Math.Round(stringSize.Width);
            _ = (int)Math.Round(stringSize.Height);
            x = e.Bounds.X + (e.Bounds.Width - stringWidth - TilePadding.Horizontal) / 2 + TilePadding.Left;
            y = e.Bounds.Y + TileSize.Height + verticalSpacing + TilePadding.Top;
            clipReg = new Region(e.Bounds);
            e.Graphics.Clip = clipReg;
            if (e.Item.Selected)
            {
                if (Focused)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, x - 1, y - 1, stringWidth + 2, stringSize.Height + 2);
                    e.Graphics.DrawString(text, Font, SystemBrushes.HighlightText, x, y);
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.ButtonFace, x - 1, y - 1, stringWidth + 2, stringSize.Height + 2);
                    e.Graphics.DrawString(text, Font, SystemBrushes.ControlText, x, y);
                }
            }
            else
            {
                e.Graphics.DrawString(text, Font, SystemBrushes.ControlText, x, y);
            }
        }
    }
    public class IconListViewItem : ListViewItem
    {
        private Icon _icon;
        public Icon Icon
        {
            get => _icon;
            set => _icon = value;
        }
    }
}
