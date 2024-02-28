jQuery(() => {
    const theTitle = jQuery("h1");
    const theNewQuoteButton = jQuery("#btn_new_quote");
    const theTable = jQuery("table");
    const tableBody = theTable.find("tbody");

    theNewQuoteButton.on("click", () => {
        window.location.href = `https://localhost:7049/new-quote?customer_id=${window.CUSTOMER_ID}`;
    });

    const renderTable = (last5Quotes) => {
        if (!last5Quotes || last5Quotes.length < 1) {
            return;
        }

        tableBody.html("");

        last5Quotes.forEach(q => {
            const theRow = jQuery("<tr>" +
                `<td>${q.dateCreated}</td>` +
                `<td>${q.riskVariationName}</td>` +
                `<td>${q.policyLineDefinitionName}</td>` +
                `<td>${q.cheapestCarrier}</td>` +
                `<td>${q.cheapestProductName}</td>` +
                `<td>£${q.cheapestPremium}</td>` +
                "</tr>");

            tableBody.append(theRow);
        });

        theTable.show();
    };

    jQuery
        .get("https://localhost:7270/customer?customer_id=" + window.CUSTOMER_ID)
        .done(viewModel => {
            theTitle.html(`Customer - ${viewModel.customerName} (${viewModel.customerLookupCode})`);
            renderTable(viewModel.last5Quotes);
        })
        .fail((jqXHR) => {
            alert(jqXHR.responseText);
        });
});