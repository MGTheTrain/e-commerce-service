/**
 * e-commerce-service
 * API documentation for the e-commerce-service
 *
 * OpenAPI spec version: v1
 * Contact: placeholder@gmail.com
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */

export interface ProductRequestDTO { 
    categories: Array<string>;
    name: string;
    description?: string;
    price: number;
    stock: number;
}