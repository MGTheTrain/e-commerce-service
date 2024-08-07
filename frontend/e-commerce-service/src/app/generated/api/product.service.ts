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
 *//* tslint:disable:no-unused-variable member-ordering */

import { Inject, Injectable, Optional }                      from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams,
         HttpResponse, HttpEvent }                           from '@angular/common/http';
import { CustomHttpUrlEncodingCodec }                        from '../encoder';

import { Observable }                                        from 'rxjs';

import { ProblemDetails } from '../model/problemDetails';
import { ProductResponseDTO } from '../model/productResponseDTO';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';


@Injectable()
export class ProductService {

    protected basePath = '/';
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(protected httpClient: HttpClient, @Optional()@Inject(BASE_PATH) basePath: string, @Optional() configuration: Configuration) {
        if (basePath) {
            this.basePath = basePath;
        }
        if (configuration) {
            this.configuration = configuration;
            this.basePath = basePath || configuration.basePath || this.basePath;
        }
    }

    /**
     * @param consumes string[] mime-types
     * @return true: consumes contains 'multipart/form-data', false: otherwise
     */
    private canConsumeForm(consumes: string[]): boolean {
        const form = 'multipart/form-data';
        for (const consume of consumes) {
            if (form === consume) {
                return true;
            }
        }
        return false;
    }


    /**
     * Retrieves all products with optional pagination and filtering.
     * 
     * @param pageNumber The page number for pagination.
     * @param pageSize The number of items per page.
     * @param category The category to filter by.
     * @param name The name to filter by.
     * @param minPrice The minimum price to filter by.
     * @param maxPrice The maximum price to filter by.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ProductsGet(pageNumber?: number, pageSize?: number, category?: string, name?: string, minPrice?: number, maxPrice?: number, observe?: 'body', reportProgress?: boolean): Observable<Array<ProductResponseDTO>>;
    public apiV1ProductsGet(pageNumber?: number, pageSize?: number, category?: string, name?: string, minPrice?: number, maxPrice?: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<ProductResponseDTO>>>;
    public apiV1ProductsGet(pageNumber?: number, pageSize?: number, category?: string, name?: string, minPrice?: number, maxPrice?: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<ProductResponseDTO>>>;
    public apiV1ProductsGet(pageNumber?: number, pageSize?: number, category?: string, name?: string, minPrice?: number, maxPrice?: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {







        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (pageNumber !== undefined && pageNumber !== null) {
            queryParameters = queryParameters.set('pageNumber', <any>pageNumber);
        }
        if (pageSize !== undefined && pageSize !== null) {
            queryParameters = queryParameters.set('pageSize', <any>pageSize);
        }
        if (category !== undefined && category !== null) {
            queryParameters = queryParameters.set('category', <any>category);
        }
        if (name !== undefined && name !== null) {
            queryParameters = queryParameters.set('name', <any>name);
        }
        if (minPrice !== undefined && minPrice !== null) {
            queryParameters = queryParameters.set('minPrice', <any>minPrice);
        }
        if (maxPrice !== undefined && maxPrice !== null) {
            queryParameters = queryParameters.set('maxPrice', <any>maxPrice);
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<Array<ProductResponseDTO>>('get',`${this.basePath}/api/v1/products`,
            {
                params: queryParameters,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Creates a new product.
     * 
     * @param categories 
     * @param name 
     * @param description 
     * @param price 
     * @param stock 
     * @param files 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ProductsPostForm(categories?: Array<string>, name?: string, description?: string, price?: number, stock?: number, files?: Array<Blob>, observe?: 'body', reportProgress?: boolean): Observable<ProductResponseDTO>;
    public apiV1ProductsPostForm(categories?: Array<string>, name?: string, description?: string, price?: number, stock?: number, files?: Array<Blob>, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ProductResponseDTO>>;
    public apiV1ProductsPostForm(categories?: Array<string>, name?: string, description?: string, price?: number, stock?: number, files?: Array<Blob>, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ProductResponseDTO>>;
    public apiV1ProductsPostForm(categories?: Array<string>, name?: string, description?: string, price?: number, stock?: number, files?: Array<Blob>, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {







        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
            'multipart/form-data'
        ];

        const canConsumeForm = this.canConsumeForm(consumes);

        let formParams: { append(param: string, value: any): void; };
        let useForm = false;
        let convertFormParamsToString = false;
        // use FormData to transmit files using content-type "multipart/form-data"
        // see https://stackoverflow.com/questions/4007969/application-x-www-form-urlencoded-or-multipart-form-data
        useForm = canConsumeForm;
        if (useForm) {
            formParams = new FormData();
        } else {
            formParams = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        }

        if (categories) {
            categories.forEach((element) => {
                formParams = formParams.append('Categories', <any>element) as any || formParams;
            })
        }
        if (name !== undefined) {
            formParams = formParams.append('Name', <any>name) as any || formParams;
        }
        if (description !== undefined) {
            formParams = formParams.append('Description', <any>description) as any || formParams;
        }
        if (price !== undefined) {
            formParams = formParams.append('Price', <any>price) as any || formParams;
        }
        if (stock !== undefined) {
            formParams = formParams.append('Stock', <any>stock) as any || formParams;
        }
        if (files) {
            files.forEach((element) => {
                formParams = formParams.append('files', <any>element) as any || formParams;
            })
        }

        return this.httpClient.request<ProductResponseDTO>('post',`${this.basePath}/api/v1/products`,
            {
                body: convertFormParamsToString ? formParams.toString() : formParams,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Deletes a product by its ID.
     * 
     * @param productId The ID of the product to delete.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ProductsProductIdDelete(productId: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public apiV1ProductsProductIdDelete(productId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public apiV1ProductsProductIdDelete(productId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public apiV1ProductsProductIdDelete(productId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (productId === null || productId === undefined) {
            throw new Error('Required parameter productId was null or undefined when calling apiV1ProductsProductIdDelete.');
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<any>('delete',`${this.basePath}/api/v1/products/${encodeURIComponent(String(productId))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Retrieves a product by its ID.
     * 
     * @param productId The ID of the product.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ProductsProductIdGet(productId: string, observe?: 'body', reportProgress?: boolean): Observable<ProductResponseDTO>;
    public apiV1ProductsProductIdGet(productId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ProductResponseDTO>>;
    public apiV1ProductsProductIdGet(productId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ProductResponseDTO>>;
    public apiV1ProductsProductIdGet(productId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (productId === null || productId === undefined) {
            throw new Error('Required parameter productId was null or undefined when calling apiV1ProductsProductIdGet.');
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<ProductResponseDTO>('get',`${this.basePath}/api/v1/products/${encodeURIComponent(String(productId))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Updates an existing product.
     * 
     * @param productId The ID of the product to update.
     * @param categories 
     * @param name 
     * @param description 
     * @param price 
     * @param stock 
     * @param files 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ProductsProductIdPutForm(productId: string, categories?: Array<string>, name?: string, description?: string, price?: number, stock?: number, files?: Array<Blob>, observe?: 'body', reportProgress?: boolean): Observable<ProductResponseDTO>;
    public apiV1ProductsProductIdPutForm(productId: string, categories?: Array<string>, name?: string, description?: string, price?: number, stock?: number, files?: Array<Blob>, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ProductResponseDTO>>;
    public apiV1ProductsProductIdPutForm(productId: string, categories?: Array<string>, name?: string, description?: string, price?: number, stock?: number, files?: Array<Blob>, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ProductResponseDTO>>;
    public apiV1ProductsProductIdPutForm(productId: string, categories?: Array<string>, name?: string, description?: string, price?: number, stock?: number, files?: Array<Blob>, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (productId === null || productId === undefined) {
            throw new Error('Required parameter productId was null or undefined when calling apiV1ProductsProductIdPut.');
        }







        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
            'multipart/form-data'
        ];

        const canConsumeForm = this.canConsumeForm(consumes);

        let formParams: { append(param: string, value: any): void; };
        let useForm = false;
        let convertFormParamsToString = false;
        // use FormData to transmit files using content-type "multipart/form-data"
        // see https://stackoverflow.com/questions/4007969/application-x-www-form-urlencoded-or-multipart-form-data
        useForm = canConsumeForm;
        if (useForm) {
            formParams = new FormData();
        } else {
            formParams = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        }

        if (categories) {
            categories.forEach((element) => {
                formParams = formParams.append('Categories', <any>element) as any || formParams;
            })
        }
        if (name !== undefined) {
            formParams = formParams.append('Name', <any>name) as any || formParams;
        }
        if (description !== undefined) {
            formParams = formParams.append('Description', <any>description) as any || formParams;
        }
        if (price !== undefined) {
            formParams = formParams.append('Price', <any>price) as any || formParams;
        }
        if (stock !== undefined) {
            formParams = formParams.append('Stock', <any>stock) as any || formParams;
        }
        if (files) {
            files.forEach((element) => {
                formParams = formParams.append('files', <any>element) as any || formParams;
            })
        }

        return this.httpClient.request<ProductResponseDTO>('put',`${this.basePath}/api/v1/products/${encodeURIComponent(String(productId))}`,
            {
                body: convertFormParamsToString ? formParams.toString() : formParams,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Retrieves the product associated with a specific user.  Explicitly checks whether a product belongs to a user by requiring a product id.
     * 
     * @param productId The ID of the product.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ProductsProductIdUserGet(productId: string, observe?: 'body', reportProgress?: boolean): Observable<ProductResponseDTO>;
    public apiV1ProductsProductIdUserGet(productId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ProductResponseDTO>>;
    public apiV1ProductsProductIdUserGet(productId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ProductResponseDTO>>;
    public apiV1ProductsProductIdUserGet(productId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (productId === null || productId === undefined) {
            throw new Error('Required parameter productId was null or undefined when calling apiV1ProductsProductIdUserGet.');
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<ProductResponseDTO>('get',`${this.basePath}/api/v1/products/${encodeURIComponent(String(productId))}/user`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Retrieves the products created by a specific user.
     * 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ProductsUserGet(observe?: 'body', reportProgress?: boolean): Observable<Array<ProductResponseDTO>>;
    public apiV1ProductsUserGet(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<ProductResponseDTO>>>;
    public apiV1ProductsUserGet(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<ProductResponseDTO>>>;
    public apiV1ProductsUserGet(observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<Array<ProductResponseDTO>>('get',`${this.basePath}/api/v1/products/user`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

}
