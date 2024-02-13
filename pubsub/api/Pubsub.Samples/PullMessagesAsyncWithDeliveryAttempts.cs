// Copyright 2020 Google Inc.
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

// [START pubsub_dead_letter_delivery_attempt]

using Google.Cloud.PubSub.V1;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class PullMessagesAsyncWithDeliveryAttemptsSample
{
    public async Task<int> PullMessagesAsyncWithDeliveryAttempts(string projectId, string subscriptionId, bool acknowledge)
    {
        // This is an existing subscription with a dead letter policy.
        SubscriptionName subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);

        SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscriptionName);

        int deliveryAttempt = 0;
        Task startTask = subscriber.StartAsync((PubsubMessage message, CancellationToken cancel) =>
        {
            string text = System.Text.Encoding.UTF8.GetString(message.Data.ToArray());
            System.Console.WriteLine($"Delivery Attempt: {message.GetDeliveryAttempt()}");
            if (message.GetDeliveryAttempt() != null)
            {
                deliveryAttempt = message.GetDeliveryAttempt().Value;
            }
            return Task.FromResult(acknowledge ? SubscriberClient.Reply.Ack : SubscriberClient.Reply.Nack);
        });
        // Run for 7 seconds.
        await Task.Delay(7000);
        await subscriber.StopAsync(CancellationToken.None);
        // Lets make sure that the start task finished successfully after the call to stop.
        await startTask;
        return deliveryAttempt;
    }
}
// [END pubsub_dead_letter_delivery_attempt]
