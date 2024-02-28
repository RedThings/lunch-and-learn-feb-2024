jQuery(() => {
    const theTitle = jQuery("h1");
    const theMessage = jQuery("#message");
    const theNumberFetched = jQuery("#number_fetched")
    const theTable = jQuery("table");
    const theTableBody = jQuery("table > tbody")
    const theUrl = "https://localhost:7270/rating-results?customer_id=" +
        window.CUSTOMER_ID +
        "&quote_id=" +
        window.QUOTE_ID +
        "&risk_variation_id=" +
        window.RISK_VARIATION_ID +
        "&policy_line_definition_code=" +
        window.POLICY_LINE_DEFINITION_CODE +
        "&rating_session_id=" +
        window.RATING_SESSION_ID;

    let rates = [];

    const renderRates = () => {
        if (rates.length < 1) {
            return;
        }

        let newTableBodyHtml = "";

        rates.forEach(rate => {
            newTableBodyHtml += "<tr>" +
                "<td>" + rate.carrier + "</td>" +
                "<td>" + rate.name + "</td>" +
                "<td>£" + rate.premium + "</td>" +
                "</tr>";
        });

        theTableBody.html(newTableBodyHtml);
        theTable.show();
    };

    const doFetch = () => {
        jQuery
            .get(theUrl)
            .done(viewModel => {
                theTitle.html("Results for '" + viewModel.riskVariationName + "' (" + viewModel.policyLineDefinitionName + ")");
                theNumberFetched.html(`${viewModel.rates.length}/${viewModel.numberOfRatesExpected} rates fetched`)

                rates = viewModel.rates;
                
                renderRates();

                if (viewModel.finishedRating === true) {
                    theMessage.html("Rating complete");
                    return;
                }

                var currentMessage = theMessage.text();
                theMessage.html(currentMessage + ".");

                window.setTimeout(doFetch, 500);
            })
            .fail((jqXHR) => {
                alert(jqXHR.responseText);
            });
    };

    doFetch();
});