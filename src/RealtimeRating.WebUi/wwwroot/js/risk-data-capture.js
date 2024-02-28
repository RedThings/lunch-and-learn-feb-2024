jQuery(() => {
    const theUrl = "https://localhost:7270/risk-data-capture" +
        "?customer_id=" +
        window.CUSTOMER_ID +
        "&quote_id=" +
        window.QUOTE_ID +
        "&risk_variation_id=" +
        window.RISK_VARIATION_ID +
        "&policy_line_definition_code=" +
        window.POLICY_LINE_DEFINITION_CODE;

    const theForm = jQuery("#the_form");
    const theFormContainer = jQuery("#inner_form");
    const thePolicyLineDefinitionName = jQuery("#policy_line_definition_name");
    const theRiskVariationName = jQuery("#risk_variation_name");

    let questions = [];

    const renderQuestions = () => {
        theFormContainer.html("");

        questions.forEach(q => {
            const row = jQuery("<div class='form-row' />");

            const label = jQuery("<label for='" + q.code + "'>" + q.label + "</label>");
            row.append(label);

            const span = jQuery("<span />");
            row.append(span);

            let validationErrorElement;

            switch (q.type) {
                case 1:
                    {
                        var input = jQuery("<input name='" + q.code + "' id='" + q.code + "' type='text' placeholder='Please enter a value...' />");

                        if (q.answer) {
                            input.val(q.answer);
                        }

                        if (q.validationError) {
                            input.addClass("validation-error");
                            validationErrorElement = jQuery("<span class='validation-error-message'>" + q.validationError + "</span>");
                            input.on("keyup", () => {
                                input.removeClass("validation-error");
                                validationErrorElement.hide();
                            });
                        }

                        span.append(input);
                        break;
                    }
                case 2:
                    {
                        var select = jQuery("<select name='" + q.code + "' id='" + q.code + "' />");

                        q.options.forEach(opt => {
                            const option = jQuery("<option value='" + opt.value + "'>" + opt.name + "</option>");
                            select.append(option);
                        });

                        if (q.answer) {
                            select.val(q.answer);
                        }

                        if (q.validationError) {
                            select.addClass("validation-error");
                            validationErrorElement = jQuery("<span class='validation-error-message'>" + q.validationError + "</span>");
                            select.on("change", () => {
                                select.removeClass("validation-error");
                                validationErrorElement.hide();
                            });
                        }

                        span.append(select);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            if (validationErrorElement) {
                row.append(validationErrorElement);
            }

            theFormContainer.append(row);
        });
    };

    const bindForm = () => {
        theForm.on("submit", e => {
            e.preventDefault();

            const theFormSerialized = theForm.serialize();

            jQuery
                .post(theUrl, theFormSerialized)
                .then(viewModel => {
                    if (viewModel.ratingStarted) {
                        const redirectPathAndQuery =
                            "/rating-results" +
                            "?customer_id=" +
                            window.CUSTOMER_ID +
                            "&quote_id=" +
                            window.QUOTE_ID +
                            "&risk_variation_id=" +
                            window.RISK_VARIATION_ID +
                            "&rating_session_id=" +
                            viewModel.ratingSessionId +
                            "&policy_line_definition_code=" +
                            window.POLICY_LINE_DEFINITION_CODE;

                        window.location.href = redirectPathAndQuery;
                        return;
                    }

                    questions = viewModel.questions;
                    renderQuestions()
                })
                .fail((jqXHR) => {
                    alert(jqXHR.responseText);
                });
        });
    };

    jQuery
        .get(theUrl)
        .done(viewModel => {
            questions = viewModel.questions;
            thePolicyLineDefinitionName.html(viewModel.policyLineDefinitionName);
            theRiskVariationName.html(viewModel.riskVariationName);
            renderQuestions();
            bindForm();
        })
        .fail((jqXHR) => {
            alert(jqXHR.responseText);
        });
});