// Copyright 2021 Google Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Threading.Tasks;
using Xunit;
using Google.Cloud.Spanner.Admin.Database.V1;

/// <summary>
/// Tests creating a databases using MR CMEK.
/// </summary>
[Collection(nameof(SpannerFixture))]
public class CreateDatabaseWithMRCMEKAsyncTest
{
    private readonly SpannerFixture _fixture;

    public CreateDatabaseWithMRCMEKAsyncTest(SpannerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task TestCreateDatabaseWithMRCMEKAsync()
    {
        // Create a database with custom encryption keys.
        var sample = new CreateDatabaseWithMRCMEKAsyncSample();
        var database = await sample.CreateDatabaseWithMRCMEKAsync(_fixture.ProjectId, _fixture.InstanceId, _fixture.EncryptedDatabaseId, _fixture.KmsKeyNames);
        Assert.Equal(_fixture.KmsKeyNames.Length, database.EncryptionConfig.KmsKeyNames.Length);
        foreach (string KmsKey in database.EncryptionConfig.KmsKeyNames)
        {
          Assert.True(_fixture.KmsKeyNames.contains(CryptoKeyName.Parse(KmsKey)));
        }
    }
}
