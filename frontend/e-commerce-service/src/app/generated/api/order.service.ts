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

import { OrderItemRequestDTO } from '../model/orderItemRequestDTO';
import { OrderItemResponseDTO } from '../model/orderItemResponseDTO';
import { OrderRequestDTO } from '../model/orderRequestDTO';
import { OrderResponseDTO } from '../model/orderResponseDTO';
import { ProblemDetails } from '../model/problemDetails';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';


@Injectable()
export class OrderService {

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
     * Retrieves all orders.
     * 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1OrdersGet(observe?: 'body', reportProgress?: boolean): Observable<Array<OrderResponseDTO>>;
    public apiV1OrdersGet(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<OrderResponseDTO>>>;
    public apiV1OrdersGet(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<OrderResponseDTO>>>;
    public apiV1OrdersGet(observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

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

        return this.httpClient.request<Array<OrderResponseDTO>>('get',`${this.basePath}/api/v1/orders`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Deletes an order by its ID.
     * 
     * @param orderId The ID of the order to delete.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1OrdersOrderIdDelete(orderId: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public apiV1OrdersOrderIdDelete(orderId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public apiV1OrdersOrderIdDelete(orderId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public apiV1OrdersOrderIdDelete(orderId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (orderId === null || orderId === undefined) {
            throw new Error('Required parameter orderId was null or undefined when calling apiV1OrdersOrderIdDelete.');
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

        return this.httpClient.request<any>('delete',`${this.basePath}/api/v1/orders/${encodeURIComponent(String(orderId))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Retrieves an order by its ID.
     * 
     * @param orderId The ID of the order.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1OrdersOrderIdGet(orderId: string, observe?: 'body', reportProgress?: boolean): Observable<OrderResponseDTO>;
    public apiV1OrdersOrderIdGet(orderId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<OrderResponseDTO>>;
    public apiV1OrdersOrderIdGet(orderId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<OrderResponseDTO>>;
    public apiV1OrdersOrderIdGet(orderId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (orderId === null || orderId === undefined) {
            throw new Error('Required parameter orderId was null or undefined when calling apiV1OrdersOrderIdGet.');
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

        return this.httpClient.request<OrderResponseDTO>('get',`${this.basePath}/api/v1/orders/${encodeURIComponent(String(orderId))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Retrieves all order items belonging to a specific order.
     * 
     * @param orderId The ID of the order.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1OrdersOrderIdItemsGet(orderId: string, observe?: 'body', reportProgress?: boolean): Observable<Array<OrderItemResponseDTO>>;
    public apiV1OrdersOrderIdItemsGet(orderId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<OrderItemResponseDTO>>>;
    public apiV1OrdersOrderIdItemsGet(orderId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<OrderItemResponseDTO>>>;
    public apiV1OrdersOrderIdItemsGet(orderId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (orderId === null || orderId === undefined) {
            throw new Error('Required parameter orderId was null or undefined when calling apiV1OrdersOrderIdItemsGet.');
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

        return this.httpClient.request<Array<OrderItemResponseDTO>>('get',`${this.basePath}/api/v1/orders/${encodeURIComponent(String(orderId))}/items`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Deletes an order item within a specific order by its ID.
     * 
     * @param orderId The ID of the order to which the item belongs.
     * @param itemId The ID of the order item to delete.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1OrdersOrderIdItemsItemIdDelete(orderId: string, itemId: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public apiV1OrdersOrderIdItemsItemIdDelete(orderId: string, itemId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public apiV1OrdersOrderIdItemsItemIdDelete(orderId: string, itemId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public apiV1OrdersOrderIdItemsItemIdDelete(orderId: string, itemId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (orderId === null || orderId === undefined) {
            throw new Error('Required parameter orderId was null or undefined when calling apiV1OrdersOrderIdItemsItemIdDelete.');
        }

        if (itemId === null || itemId === undefined) {
            throw new Error('Required parameter itemId was null or undefined when calling apiV1OrdersOrderIdItemsItemIdDelete.');
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

        return this.httpClient.request<any>('delete',`${this.basePath}/api/v1/orders/${encodeURIComponent(String(orderId))}/items/${encodeURIComponent(String(itemId))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Retrieves an order item by its ID within a specific order.
     * 
     * @param orderId The ID of the order.
     * @param itemId The ID of the order item.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1OrdersOrderIdItemsItemIdGet(orderId: string, itemId: string, observe?: 'body', reportProgress?: boolean): Observable<OrderItemResponseDTO>;
    public apiV1OrdersOrderIdItemsItemIdGet(orderId: string, itemId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<OrderItemResponseDTO>>;
    public apiV1OrdersOrderIdItemsItemIdGet(orderId: string, itemId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<OrderItemResponseDTO>>;
    public apiV1OrdersOrderIdItemsItemIdGet(orderId: string, itemId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (orderId === null || orderId === undefined) {
            throw new Error('Required parameter orderId was null or undefined when calling apiV1OrdersOrderIdItemsItemIdGet.');
        }

        if (itemId === null || itemId === undefined) {
            throw new Error('Required parameter itemId was null or undefined when calling apiV1OrdersOrderIdItemsItemIdGet.');
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

        return this.httpClient.request<OrderItemResponseDTO>('get',`${this.basePath}/api/v1/orders/${encodeURIComponent(String(orderId))}/items/${encodeURIComponent(String(itemId))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Updates an existing order item within a specific order.
     * 
     * @param orderId The ID of the order to which the item belongs.
     * @param itemId The ID of the order item to update.
     * @param body The order item data transfer object containing updated item details.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1OrdersOrderIdItemsItemIdPut(orderId: string, itemId: string, body?: OrderItemRequestDTO, observe?: 'body', reportProgress?: boolean): Observable<OrderItemResponseDTO>;
    public apiV1OrdersOrderIdItemsItemIdPut(orderId: string, itemId: string, body?: OrderItemRequestDTO, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<OrderItemResponseDTO>>;
    public apiV1OrdersOrderIdItemsItemIdPut(orderId: string, itemId: string, body?: OrderItemRequestDTO, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<OrderItemResponseDTO>>;
    public apiV1OrdersOrderIdItemsItemIdPut(orderId: string, itemId: string, body?: OrderItemRequestDTO, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (orderId === null || orderId === undefined) {
            throw new Error('Required parameter orderId was null or undefined when calling apiV1OrdersOrderIdItemsItemIdPut.');
        }

        if (itemId === null || itemId === undefined) {
            throw new Error('Required parameter itemId was null or undefined when calling apiV1OrdersOrderIdItemsItemIdPut.');
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
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.request<OrderItemResponseDTO>('put',`${this.basePath}/api/v1/orders/${encodeURIComponent(String(orderId))}/items/${encodeURIComponent(String(itemId))}`,
            {
                body: body,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Creates a new order item within an order.
     * 
     * @param orderId The ID of the order to which the item belongs.
     * @param body The order item data transfer object containing item details.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1OrdersOrderIdItemsPost(orderId: string, body?: OrderItemRequestDTO, observe?: 'body', reportProgress?: boolean): Observable<OrderItemResponseDTO>;
    public apiV1OrdersOrderIdItemsPost(orderId: string, body?: OrderItemRequestDTO, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<OrderItemResponseDTO>>;
    public apiV1OrdersOrderIdItemsPost(orderId: string, body?: OrderItemRequestDTO, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<OrderItemResponseDTO>>;
    public apiV1OrdersOrderIdItemsPost(orderId: string, body?: OrderItemRequestDTO, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (orderId === null || orderId === undefined) {
            throw new Error('Required parameter orderId was null or undefined when calling apiV1OrdersOrderIdItemsPost.');
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
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.request<OrderItemResponseDTO>('post',`${this.basePath}/api/v1/orders/${encodeURIComponent(String(orderId))}/items`,
            {
                body: body,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Updates an existing order.
     * 
     * @param orderId The ID of the order to update.
     * @param body The order data transfer object containing updated order details.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1OrdersOrderIdPut(orderId: string, body?: OrderRequestDTO, observe?: 'body', reportProgress?: boolean): Observable<OrderRequestDTO>;
    public apiV1OrdersOrderIdPut(orderId: string, body?: OrderRequestDTO, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<OrderRequestDTO>>;
    public apiV1OrdersOrderIdPut(orderId: string, body?: OrderRequestDTO, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<OrderRequestDTO>>;
    public apiV1OrdersOrderIdPut(orderId: string, body?: OrderRequestDTO, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (orderId === null || orderId === undefined) {
            throw new Error('Required parameter orderId was null or undefined when calling apiV1OrdersOrderIdPut.');
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
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.request<OrderRequestDTO>('put',`${this.basePath}/api/v1/orders/${encodeURIComponent(String(orderId))}`,
            {
                body: body,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Creates a new order.
     * 
     * @param body The order data transfer object containing order details.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1OrdersPost(body?: OrderRequestDTO, observe?: 'body', reportProgress?: boolean): Observable<OrderResponseDTO>;
    public apiV1OrdersPost(body?: OrderRequestDTO, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<OrderResponseDTO>>;
    public apiV1OrdersPost(body?: OrderRequestDTO, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<OrderResponseDTO>>;
    public apiV1OrdersPost(body?: OrderRequestDTO, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {


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
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.request<OrderResponseDTO>('post',`${this.basePath}/api/v1/orders`,
            {
                body: body,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

}