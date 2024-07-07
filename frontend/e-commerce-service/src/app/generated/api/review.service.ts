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
import { ReviewRequestDTO } from '../model/reviewRequestDTO';
import { ReviewResponseDTO } from '../model/reviewResponseDTO';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';


@Injectable()
export class ReviewService {

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
     * Retrieves all reviews.
     * 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ReviewsGet(observe?: 'body', reportProgress?: boolean): Observable<Array<ReviewResponseDTO>>;
    public apiV1ReviewsGet(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<ReviewResponseDTO>>>;
    public apiV1ReviewsGet(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<ReviewResponseDTO>>>;
    public apiV1ReviewsGet(observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

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

        return this.httpClient.request<Array<ReviewResponseDTO>>('get',`${this.basePath}/api/v1/reviews`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Creates a new review.
     * 
     * @param body The review data transfer object containing review details.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ReviewsPost(body?: ReviewRequestDTO, observe?: 'body', reportProgress?: boolean): Observable<ReviewResponseDTO>;
    public apiV1ReviewsPost(body?: ReviewRequestDTO, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ReviewResponseDTO>>;
    public apiV1ReviewsPost(body?: ReviewRequestDTO, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ReviewResponseDTO>>;
    public apiV1ReviewsPost(body?: ReviewRequestDTO, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {


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

        return this.httpClient.request<ReviewResponseDTO>('post',`${this.basePath}/api/v1/reviews`,
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
     * Retrieves reviews by product ID.
     * 
     * @param productId The ID of the product.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ReviewsProductProductIdGet(productId: string, observe?: 'body', reportProgress?: boolean): Observable<Array<ReviewResponseDTO>>;
    public apiV1ReviewsProductProductIdGet(productId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<ReviewResponseDTO>>>;
    public apiV1ReviewsProductProductIdGet(productId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<ReviewResponseDTO>>>;
    public apiV1ReviewsProductProductIdGet(productId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (productId === null || productId === undefined) {
            throw new Error('Required parameter productId was null or undefined when calling apiV1ReviewsProductProductIdGet.');
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

        return this.httpClient.request<Array<ReviewResponseDTO>>('get',`${this.basePath}/api/v1/reviews/product/${encodeURIComponent(String(productId))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Deletes a review by its ID.
     * 
     * @param reviewId The ID of the review to delete.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ReviewsReviewIdDelete(reviewId: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public apiV1ReviewsReviewIdDelete(reviewId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public apiV1ReviewsReviewIdDelete(reviewId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public apiV1ReviewsReviewIdDelete(reviewId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (reviewId === null || reviewId === undefined) {
            throw new Error('Required parameter reviewId was null or undefined when calling apiV1ReviewsReviewIdDelete.');
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

        return this.httpClient.request<any>('delete',`${this.basePath}/api/v1/reviews/${encodeURIComponent(String(reviewId))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Retrieves a review by its ID.
     * 
     * @param reviewId The ID of the review.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ReviewsReviewIdGet(reviewId: string, observe?: 'body', reportProgress?: boolean): Observable<ReviewResponseDTO>;
    public apiV1ReviewsReviewIdGet(reviewId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ReviewResponseDTO>>;
    public apiV1ReviewsReviewIdGet(reviewId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ReviewResponseDTO>>;
    public apiV1ReviewsReviewIdGet(reviewId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (reviewId === null || reviewId === undefined) {
            throw new Error('Required parameter reviewId was null or undefined when calling apiV1ReviewsReviewIdGet.');
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

        return this.httpClient.request<ReviewResponseDTO>('get',`${this.basePath}/api/v1/reviews/${encodeURIComponent(String(reviewId))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Updates an existing review.
     * 
     * @param reviewId The ID of the review to update.
     * @param body The review data transfer object containing updated review details.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiV1ReviewsReviewIdPut(reviewId: string, body?: ReviewRequestDTO, observe?: 'body', reportProgress?: boolean): Observable<ReviewResponseDTO>;
    public apiV1ReviewsReviewIdPut(reviewId: string, body?: ReviewRequestDTO, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ReviewResponseDTO>>;
    public apiV1ReviewsReviewIdPut(reviewId: string, body?: ReviewRequestDTO, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ReviewResponseDTO>>;
    public apiV1ReviewsReviewIdPut(reviewId: string, body?: ReviewRequestDTO, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (reviewId === null || reviewId === undefined) {
            throw new Error('Required parameter reviewId was null or undefined when calling apiV1ReviewsReviewIdPut.');
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

        return this.httpClient.request<ReviewResponseDTO>('put',`${this.basePath}/api/v1/reviews/${encodeURIComponent(String(reviewId))}`,
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
