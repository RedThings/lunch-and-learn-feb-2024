jQuery(() => {
    const theForm = jQuery("#the_form");
    const theNameElement = jQuery("#quote_name");
    const thePolicyTypeElement = jQuery("#policy_type");

    jQuery
        .get(`https://localhost:7270/new-quote`)
        .done(viewModel => {
            viewModel.policyLineDefinitions.forEach(pt => {
                var option = jQuery("<option value='" + pt.code + "'>" + pt.name + "</option>")
                thePolicyTypeElement.append(option);
            });
        })
        .fail((jqXHR) => {
            alert(jqXHR.responseText);
        });

    theForm.on("submit", e => {
        e.preventDefault();

        const theName = theNameElement.val();
        const thePolicyType = thePolicyTypeElement.val();

        if (!theName || !thePolicyType) {
            return;
        }

        const requestBody = { name: theName };

        jQuery
            .ajax({
                method: "POST",
                url: "https://localhost:7270/new-quote",
                data: JSON.stringify(requestBody),
                contentType: "application/json; charset=UTF-8",
                dataType: "json"
            })
            .done(viewModel => {
                window.location.href = `/risk-data-capture?customer_id=${window.CUSTOMER_ID}&quote_id=${viewModel.quoteId}&risk_variation_id=${viewModel.riskVariationId}&policy_line_definition_code=${thePolicyType}`;
            })
            .fail((jqXHR) => {
                alert(jqXHR.responseText);
            });
    });
});