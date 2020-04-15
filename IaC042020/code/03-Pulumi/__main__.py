import pulumi
from pulumi import ResourceOptions
from pulumi_azure import core, cognitive

attendees = ['alice','bob']

for i in attendees:
    resource_name = "iaac-cv-pulumi-{0}".format(i)

    # Create an Azure resource (Cognitive Services Account)
    cvaccount = cognitive.Account(resource_name,
                            # The location for the resource will be derived automatically from the resource group.
                            name=resource_name,
                            resource_group_name='iaac-rg',
                            kind='ComputerVision',
                            sku_name='S1',opts=ResourceOptions(delete_before_replace=True))

    # Export the resource endpoint for the cognitive services resource
    pulumi.export('endpoint', cvaccount.endpoint)
