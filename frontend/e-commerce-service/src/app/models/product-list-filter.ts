import { BaseFilter } from "./base-filter";

export interface ProductListFilter extends BaseFilter {
    category: string;
    minPrice: number | null;
    maxPrice: number | null;
}