# Rhema

Rhema is a .NET based Greek/Hebrew language syntax search tool designed to be used similar to Accordance, Bible Works, or other major Bible software. It is open-sourced under the MIT license.

[![Build status](https://ci.appveyor.com/api/projects/status/07dm7uq4c06ylwj6?svg=true)](https://ci.appveyor.com/project/vbnet3d/rhema)

# Examples

Syntax search using WITHIN command: 

    αυτος <WITHIN 5 WORDS> ανθρωπος
    
    αυτος <WITHIN 5 WORDS> ανθρωπος <OR> αυτου <WITHIN 5 WORDS> ανθρωπος

# Search Syntax (Work in Progress)

## COMMANDS ##

---
Form: `x <command_name options> y` 

Notes: x and y may be words or other conditions.

---
 - `x <WITHIN distance WORDS> y` 
 - `x <OR> y`  
 - `x <AND> y` 
 - `x <NOT> y` 
 - `x <XOR> y`
 - `x <FOLLOWED BY distance WORDS> y`
 - `x <PRECEDED BY distance WORDS> y` 

## PARTS OF SPEECH ##

---
 - Form: `[part_name id(numeric, optional) parsing(optional)]`

> Notes: Matching ids are used when you want to select different words
 with the basic parsing i.e. A Nom. Masc. Sg. Article and Nom. Masc. Sg. Noun, without specifying exactly what forms to return. This allows the user to search for all forms of a syntactic structure. Parsing is in the form of GNC for substantives or TVMPN for standard Greek verbs. Greek Participle parsing Is in the form of TVMGNC. Currently only Greek parsing is available.

---

 - `[ANY id parsing]`           Matches any word unit
 - `[ARTICLE id parsing]`       Matches any article
 - `[SUBSTANTIVE id parsing]`   Matches any substantive word unit
 - `[NOUN id parsing]`          Matches any noun
 - `[ANYPRONOUN id parsing]`    Matches any pronoun
 - `[PRONOUN id parsing]`       Matches only regular pronouns
 - `[REFLEXIVE id parsing]`     Matches only reflexive pronouns
 - `[INDEFINITE id parsing]`    Matches only indefinite pronouns
 - `[NUMBER id parsing]`        Matches only indeclinable numbers
 - `[PARTICLE id parsing]`      Matches only particles
 - `[VERB id parsing]`          Matches only verbs

---
###Parsing Options:###

---
  - Gender: M(asculine), F(eminine), N(euter)
  - Number: S(ingular), P(lural), D(ual)
  - Case:   N(ominative), D(ative), G(enitive), A(ccusative), V(ocative)
  - Tense:  P(resent), A(orist), X(Perfect), I(mperfect), F(uture), Y(Pluperfect)
  - Voice:  A(ctive), M(iddle), P(assive)
  - Mood:   I(ndicative), D(Imperative), (I)N(finitive), P(articiple), S(ubjunctive), O(ptative)
  - Person: 1, 2, 3



---
##Strong's Numbers ##

---
Form: `<G123>`

---
