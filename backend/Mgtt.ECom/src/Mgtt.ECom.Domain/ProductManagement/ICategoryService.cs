// <copyright file="ICategoryService.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.ProductManagement;

public interface ICategoryService
{
    Task<Category?> GetCategoryById(Guid categoryId);

    Task<IEnumerable<Category>?> GetAllCategories();

    Task<Category?> CreateCategory(Category category);

    Task<Category?> UpdateCategory(Category category);

    Task DeleteCategory(Guid categoryId);
}
