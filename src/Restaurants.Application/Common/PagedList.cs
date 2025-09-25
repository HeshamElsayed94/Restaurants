using Microsoft.EntityFrameworkCore;

namespace Restaurants.Application.Common;

public sealed class PagedList<T>
{
	private PagedList(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber, int totalPages)
	{
		Items = items;
		TotalCount = totalCount;
		PageSize = pageSize;
		PageNumber = pageNumber;
		TotalPages = totalPages;
		ItemsFrom = TotalCount > 0 ? (PageSize * (PageNumber - 1)) + 1 : 0;
		ItemsTo = TotalCount > 0 ? ItemsFrom + PageSize - 1 : 0;
	}

	public int PageSize { get; }

	public int PageNumber { get; }

	public int TotalCount { get; }

	public int TotalPages { get; }

	public int ItemsFrom { get; }

	public int ItemsTo { get; }

	public IEnumerable<T> Items { get; } = default!;

	public static async Task<PagedList<T>> Create(IQueryable<T> query, int pageSize, int pageNumber, CancellationToken ct = default)
	{
		var totalCount = await query.CountAsync(ct);

		var pagingInfo = GetPagingInfo(totalCount, pageSize, pageNumber);

		var skippedCount = pagingInfo.pageSize * (pagingInfo.pageNumber - 1);

		var items = await query.Skip(skippedCount).Take(pagingInfo.pageSize).ToListAsync(ct);

		return new(items, totalCount, pagingInfo.pageSize, pagingInfo.pageNumber, pagingInfo.toalPages);
	}

	public static PagedList<T> Create(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
	{
		var pagingInfo = GetPagingInfo(totalCount, pageSize, pageNumber);

		return new(items, totalCount, pagingInfo.pageSize, pagingInfo.pageNumber, pagingInfo.toalPages);
	}

	private static (int toalPages, int pageSize, int pageNumber) GetPagingInfo(int totalCount, int pageSize, int pageNumber)
	{
		pageSize = Math.Clamp(pageSize, 1, 100);

		var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));

		pageNumber = Math.Clamp(pageNumber, 1, totalPages);

		return (totalCount, pageSize, pageNumber);
	}
}