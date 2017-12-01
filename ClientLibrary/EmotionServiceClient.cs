// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services (formerly Project Oxford): https://www.microsoft.com/cognitive-services
// 
// Microsoft Cognitive Services (formerly Project Oxford) GitHub:
// https://github.com/Microsoft/Cognitive-Emotion-Windows
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.ProjectOxford.Common;
using Newtonsoft.Json;

namespace Microsoft.ProjectOxford.Emotion
{
    public class EmotionServiceClient : ServiceClient, IEmotionServiceClient
    {
        /// <summary>
        /// Default service endpoint URL root.
        /// </summary>
        public const string DEFAULT_API_ROOT = "https://westus.api.cognitive.microsoft.com/emotion/v1.0";

        /// <summary>
        /// Initializes a new instance of the <see cref="EmotionServiceClient"/> class.
        /// </summary>
        /// <param name="subscriptionKey">The subscription key.</param>
        public EmotionServiceClient(string subscriptionKey) : this(subscriptionKey, DEFAULT_API_ROOT) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EmotionServiceClient"/> class.
        /// </summary>
        /// <param name="subscriptionKey">The subscription key.</param>
        /// <param name="apiRoot">Root URL of the service endpoint.</param>
        public EmotionServiceClient(string subscriptionKey, string apiRoot) : base()
        {
            ApiRoot = apiRoot?.TrimEnd('/');
            AuthKey = "Ocp-Apim-Subscription-Key";
            AuthValue = subscriptionKey;
        }

        #region IEmotionServiceClient implementations
        /// <summary>
        /// Recognize emotions on faces in an image.
        /// </summary>
        /// <param name="imageUrl">URL of the image.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Async task, which, upon completion, will return rectangle and emotion scores for each recognized face.</returns>
        public Task<Microsoft.ProjectOxford.Common.Contract.Emotion[]> RecognizeAsync(String imageUrl, CancellationToken cancellationToken = default(CancellationToken))
        {
            return RecognizeAsync(imageUrl, null);
        }

        /// <summary>
        /// Recognize emotions on faces in an image.
        /// </summary>
        /// <param name="imageUrl">URL of the image.</param>
        /// <param name="faceRectangles">Array of face rectangles.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Async task, which, upon completion, will return rectangle and emotion scores for each recognized face.</returns>
        public Task<Microsoft.ProjectOxford.Common.Contract.Emotion[]> RecognizeAsync(String imageUrl, Rectangle[] faceRectangles, CancellationToken cancellationToken = default(CancellationToken))
        {
            return PostAsync<UrlReqeust, Microsoft.ProjectOxford.Common.Contract.Emotion[]>(GetRecognizeUrl(faceRectangles), new UrlReqeust { url = imageUrl }, cancellationToken);
        }

        /// <summary>
        /// Recognize emotions on faces in an image.
        /// </summary>
        /// <param name="imageStream">Stream of the image</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Async task, which, upon completion, will return rectangle and emotion scores for each recognized face.</returns>
        public Task<Microsoft.ProjectOxford.Common.Contract.Emotion[]> RecognizeAsync(Stream imageStream, CancellationToken cancellationToken = default(CancellationToken))
        {
            return RecognizeAsync(imageStream, null);
        }

        /// <summary>
        /// Recognize emotions on faces in an image.
        /// </summary>
        /// <param name="imageStream">Stream of the image</param>
        /// <param name="faceRectangles">Array of face rectangles.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Async task, which, upon completion, will return rectangle and emotion scores for each face.</returns>
        public Task<Microsoft.ProjectOxford.Common.Contract.Emotion[]> RecognizeAsync(Stream imageStream, Rectangle[] faceRectangles, CancellationToken cancellationToken = default(CancellationToken))
        {
            return PostAsync<Stream, Microsoft.ProjectOxford.Common.Contract.Emotion[]>(GetRecognizeUrl(faceRectangles), imageStream, cancellationToken);
        }

        private string GetRecognizeUrl(Rectangle[] faceRectangles)
        {
            var builder = new StringBuilder("/recognize");
            if (faceRectangles != null && faceRectangles.Length > 0)
            {
                builder.Append("?faceRectangles=");
                builder.Append(string.Join(";", faceRectangles.Select(r => String.Format("{0},{1},{2},{3}", r.Left, r.Top, r.Width, r.Height))));
            }
            return builder.ToString();
        }
        #endregion
    }
}
