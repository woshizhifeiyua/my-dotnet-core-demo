﻿using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyDemoIdentityServer.Tool
{
    public class WeiXinOpenGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "weiopenid";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            try
            {
                #region 参数获取 直接把授权后的openId 拿过来授权是不安全的，这里仅仅是一个Demo
                var openId = context.Request.Raw["openid"];
                var unionId = context.Request.Raw["UnionId"];
                var userName = context.Request.Raw["userName"];
                #endregion

                #region 通过openId和unionId 参数来进行数据库的相关验证
                var claimList = new List<Claim>() { };  // ValidateUserAsync(openId, unionId);
                #endregion

                #region 授权通过
                //授权通过返回
                context.Result = new GrantValidationResult
                (
                    subject: openId,
                    authenticationMethod: "custom",
                    claims: claimList.ToArray()
                );
                #endregion
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult()
                {
                    IsError = true,
                    Error = ex.Message
                };
            }
        }


    }
}
