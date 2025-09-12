# Test Summary

**Table of Contents**

- [Category: Core](#category-core)
- [Core: Login](#core-login)
- [No Trait](#no-trait)
- [SmokeTest: 2](#smoketest-2)
- [Domain: Documents](#domain-documents)
- [SmokeTest: 1](#smoketest-1)
- [Document: Create](#document-create)
- [Business: OOTB](#business-ootb)
- [Domain: ECO](#domain-eco)
- [ECO: Create](#eco-create)
- [Domain: Part](#domain-part)
- [Part: Create](#part-create)
- [Part: Release](#part-release)

## Domain: Documents

- `Admin_can_find_a_Document`
- `Admin_can_create_Document`
- `Admin_can_NOT_create_Document_without_ItemNumber`

## Domain: ECO

- `Admin_can_find_ECO`
- `Admin_can_create_ECO`
- `Admin_can_NOT_create_ECO_without_a_Title`

## Domain: Part

- `Admin_can_find_Part`
- `Admin_can_create_Part`
- `Users_can_manually_Release_Part`
- `CM_can_delete_a_new_Part`
- `CM_can_NOT_delete_Part_when_Released`
- `User_can_NOT_edit_Part_when_locked_by_another_user`
- `CM_can_NOT_edit_Part_when_Released`
- `CM_can_create_new_revision_of_Part_when_Released`

## Category: Core

- `LoginWithFixture_ShouldHaveALoggedInUser`
- `Admin_can_find_a_Document`
- `Admin_can_find_ECO`
- `Admin_can_find_Part`

## Core: Login

- `LoginWithFixture_ShouldHaveALoggedInUser`

## No Trait

- `TestLab`
- `CM_can_Release_an_Item_via_ECO`

## SmokeTest: 2

- `Admin_can_add_a_File_to_vault`

## SmokeTest: 1

- `Admin_can_find_a_Document`
- `Admin_can_find_ECO`
- `Admin_can_find_Part`

## Document: Create

- `Admin_can_create_Document`
- `Admin_can_NOT_create_Document_without_ItemNumber`

## Business: OOTB

- `Admin_can_create_Document`
- `Admin_can_NOT_create_Document_without_ItemNumber`
- `Admin_can_create_ECO`
- `Admin_can_NOT_create_ECO_without_a_Title`
- `CM_can_delete_a_new_Part`
- `CM_can_NOT_delete_Part_when_Released`
- `User_can_NOT_edit_Part_when_locked_by_another_user`
- `CM_can_NOT_edit_Part_when_Released`
- `CM_can_create_new_revision_of_Part_when_Released`

## ECO: Create

- `Admin_can_create_ECO`
- `Admin_can_NOT_create_ECO_without_a_Title`

## Part: Create

- `Admin_can_create_Part`

## Part: Release

- `Users_can_manually_Release_Part`

