﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Ultimate.SharedCommon.Dtos
{
    //Generic bir Reponse yapısı uyguladık ve bu Response partterninde . Hata alan yerler Fail ve Başarılı olan yerlerde Success
    //Static Factory Yapısını kullanacağız
    public class Response<T>
    {
        public T Data { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }

        //Static Factory Method
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T>() { Data = data, StatusCode = statusCode, IsSuccess = true };
        }
        public static Response<T> Success(int statusCode)
        {
            return new Response<T>() { Data = default(T), StatusCode = statusCode, IsSuccess = true };
        }
        public static Response<T> Fail(List<string> errors,int statusCode)
        {
            return new Response<T>() { Errors = errors, StatusCode = statusCode, IsSuccess=false };
        }
        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T>() { Errors=new List<string>() { error}, StatusCode = statusCode,IsSuccess=false };
        }
    }
}
