﻿using System;

namespace Chiota.Controls.InfiniteScrolling
{
	public class LoadingMoreEventArgs : EventArgs
	{
		public LoadingMoreEventArgs(bool isLoadingMore)
		{
			IsLoadingMore = isLoadingMore;
		}

		public bool IsLoadingMore { get; }
	}
}
