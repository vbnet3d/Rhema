# Rhema

Rhema is a .NET based Greek/Hebrew language syntax search tool designed to be used similar to Accordance, Bible Works, or other major Bible software. It is open-sourced under the MIT license.

[![Build status](https://ci.appveyor.com/api/projects/status/07dm7uq4c06ylwj6?svg=true)](https://ci.appveyor.com/project/vbnet3d/rhema)

# Examples

Syntax search using WITHIN command: 

    αυτος <WITHIN 5 WORDS> ανθρωπος

# Search Syntax (Work in Progress)

## COMMANDS
---
Form: `x <command_name options> y`
Notes: x and y may be WORDS or other conditions.
---
`x <WITHIN distance WORDS> y`       Is word `x` within `distance` words of `y`, either direction?
`x <OR> y`                          Either `x` or `y` may be true
`x <AND> y`                        `x` and `y` must be true
`x <NOT> y`                         x must be true and y false
`x <XOR> y`                         Only one of x/y may be true
`x <FOLLOWED BY distance WORDS> y`  Is word `x` followed by `y` within `distance`?
`x <PRECEDED BY distance WORDS> y`  Is word `x` preceded by `y` within `distance`?

## PARTS OF SPEECH
---
Form: `[part_name id(numeric, optional) parsing(optional)]`
Notes: Matching ids are used when you want to select different words with the basic parsing
       i.e. A Nom. Masc. Sg. Article and Nom. Masc. Sg. Noun, without specifying exactly what
       forms to return. This allows the user to search for all forms of a syntactic structure.
       Parsing is in the form of G(ender)N(umber)C(ase) for substantives or 
       T(ense)V(oice)M(ood)P(erson)N(umber) for standard Greek verbs. Greek Participle parsing
       Is in the form of T(ense)V(oice)M(ood)G(ender)N(umber)C(ase). Hebrew parsing will be
       added later.
---
Parsing Options:
   Gender: M(asculine), F(eminine), N(euter)
   Number: S(ingular), P(lural), D(ual)
   Case:   N(ominative), D(ative), G(enitive), A(ccusative), V(ocative)
   Tense:  P(resent), A(orist), X(Perfect), I(mperfect), F(uture), Y(Pluperfect)
   Voice:  A(ctive), M(iddle), P(assive)
   Mood:   I(ndicative), D(Imperative), (I)N(finitive), P(articiple), S(ubjunctive), O(ptative)
   Person: 1, 2, 3
---
`[ANY id parsing]`           Matches any word unit
`[ARTICLE id parsing]`       Matches any article
`[SUBSTANTIVE id parsing]`   Matches any substantive word unit
`[NOUN id parsing]`          Matches any noun
`[ANYPRONOUN id parsing]`    Matches any pronoun
`[PRONOUN id parsing]`       Matches only regular pronouns
`[REFLEXIVE id parsing]`     Matches only reflexive pronouns
`[INDEFINITE id parsing]`    Matches only indefinite pronouns
`[NUMBER id parsing]`        Matches only indeclinable numbers
`[PARTICLE id parsing]`      Matches only particles
`[VERB id parsing]`          Matches only verbs
---
##Strong's Numbers
---
Form: `<G123>`
---