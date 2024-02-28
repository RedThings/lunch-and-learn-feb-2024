jQuery(() => {
    const theCustomerElement = jQuery("#customer");

    theCustomerElement.on("change", () => {
        var customerId = theCustomerElement.val();

        if (!customerId) {
            return;
        }

        var theUrl = "https://localhost:7049/customer?customer_id=" + customerId;

        window.location.href = theUrl;
    });

    jQuery
        .get("https://localhost:7270/")
        .done(customers => {
            customers.forEach(customer => {
                var option = jQuery("<option value='" + customer.id + "'>" + customer.name + "</option>")
                theCustomerElement.append(option);
            });
        })
        .fail((jqXHR) => {
            alert(jqXHR.responseText);
        });
});