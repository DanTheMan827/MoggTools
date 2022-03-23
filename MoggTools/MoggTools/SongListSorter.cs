using System;
using System.Collections;
using System.Windows.Forms;

namespace MoggTools
{
	public class SongListSorter : IComparer
	{
		private int ColumnToSort;

		private SortOrder OrderOfSort;

		private CaseInsensitiveComparer ObjectCompare;

		public SortOrder Order
		{
			get
			{
				return this.OrderOfSort;
			}
			set
			{
				this.OrderOfSort = value;
			}
		}

		public int SortColumn
		{
			get
			{
				return this.ColumnToSort;
			}
			set
			{
				this.ColumnToSort = value;
			}
		}

		public SongListSorter()
		{
			this.ColumnToSort = 0;
			this.OrderOfSort = SortOrder.None;
			this.ObjectCompare = new CaseInsensitiveComparer();
		}

		public int Compare(object x, object y)
		{
			ListViewItem listViewItem = (ListViewItem)x;
			ListViewItem listViewItem1 = (ListViewItem)y;
			int num = this.ObjectCompare.Compare(listViewItem.SubItems[this.ColumnToSort].Text, listViewItem1.SubItems[this.ColumnToSort].Text);
			if (this.OrderOfSort == SortOrder.Ascending)
			{
				return num;
			}
			if (this.OrderOfSort != SortOrder.Descending)
			{
				return 0;
			}
			return -num;
		}
	}
}