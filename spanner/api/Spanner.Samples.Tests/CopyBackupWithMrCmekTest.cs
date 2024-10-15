// Copyright 2024 Google Inc.
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

using Google.Cloud.Spanner.Admin.Database.V1;
using System;
using Xunit;

[Collection(nameof(SpannerFixture))]
public class CopyBackupWithMrCmekTest
{
    private readonly SpannerFixture _spannerFixture;

    public CopyBackupWithMrCmekTest(SpannerFixture spannerFixture)
    {
        _spannerFixture = spannerFixture;
    }

    [Fact]
    public void CopyBackupWithMrCmek()
    {
        CopyBackupWithMrCmekSample copyBackupWithMrCmekSample = new CopyBackupWithMrCmekSample();
        string source_Project_id = _spannerFixture.ProjectId;
        string source_Instance_id = _spannerFixture.InstanceId;
        string source_backupId = _spannerFixture.MrCmekBackupId;
        string target_Project_id = _spannerFixture.ProjectId;
        string target_Instance_id = _spannerFixture.InstanceId;
        string target_backupId = SpannerFixture.GenerateId("test_", 16);
        DateTimeOffset expireTime = DateTimeOffset.UtcNow.AddHours(12);
        CryptoKeyName[] kmsKeyNames = _fixture.KmsKeyNames;

        Backup backup = copyBackupWithMrCmekSample.CopyBackupWithMrCmek(source_Instance_id, source_Project_id, source_backupId, 
            target_Instance_id, target_Project_id, target_backupId, expireTime, kmsKeyNames);

        Assert.All(backup.EncryptionInfo.KmsKeyVersionsAsCryptoKeyVersionNames, keyName => _fixture.KmsKeyNames.Contains(keyName.CryptoKeyId));
    }
}
