import { BaseFilter } from "./base-filter.model";

export interface ProductListFilter extends BaseFilter {
    category: string | null;
    minPrice: number | null;
    maxPrice: number | null;
}