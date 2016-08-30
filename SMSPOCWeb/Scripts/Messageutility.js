        var LoadSubscriberSMS = function (successcallback,isasync) {
            $.ajax({
                type: 'Post',
                async: isasync,
                url: '/Notify/GetSubscriberSMS',
                dataType: 'json',
                success: function (data, success) {
                    if(successcallback)
                    successcallback(data);
                },
                error: function (data) {
                    alert('problem in retrieving message balance details');
                }
            });
        };
    
